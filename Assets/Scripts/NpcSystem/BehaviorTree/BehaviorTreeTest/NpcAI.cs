using BehaviorTreeTest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using static UnityEngine.Rendering.DebugUI;

public class NpcAI : MonoBehaviour, IHasTarget, IEmotable, IMoodController, IDespawnable, IHasDisplayChoices, IHasDisplayTarget, IHaggler
{
    private BTree _tree;
    private Blackboard _blackboard;

    #region Serialized Fields
    [SerializeField] private float _toleranceDecimal;
    [SerializeField] private NPCType _NPCType;
    #endregion

    #region Private Variables
    private NavMeshAgent _agent;
    private Vector3 _despawnPointPos;
    private Vector3 _windowPos;
    private Vector3 _doorPos;
    private Vector3 _despawnInShop;
    private Vector3 _shopSpawnPoint;
    private List<Vector3> _counterPos;
    private Vector3 _target;
    private List<ItemData> _desiredItems;
    private DisplayContext _displayTarget;
    private List<DisplayContext> _possibleDisplayChoices;
    private IObjectPool<NpcAI> _pool;
    private bool _isHaggling;
    #endregion

    ILinePositionManager _linePositionManager;
    INPCReadyToHaggleController _readyToHaggleController;

    #region Properties
    public Vector3 Target { get => _target; set => _target=value; }
    public List<DisplayContext> PossibleDisplayChoices
    {
        get => _possibleDisplayChoices; set => _possibleDisplayChoices=value; }
    public DisplayContext DisplayTarget { get => _displayTarget; set => _displayTarget=value; }
    public bool IsHaggling { get => _isHaggling; set { _blackboard.Set("isHaggling", value); _isHaggling = value; } }

    #endregion

    #region events
    public event Action<MoodType> MoodChanged;
    #endregion

    #region UI stuff
    [SerializeField] private UpdateEmote _updateEmote;
    private NPCEmotePresenter _presenter;
    #endregion
    public void Initialize(bool isInShop, Vector3 spawnPoint, Vector3 despawnPointPos, Vector3 despawnInShop, Vector3 windowPos, Vector3 doorPos, Vector3 shopSpawnPoint, List<Vector3> counterPos, List<ItemData> desiredItems, ILinePositionManager linePositionManager, INPCReadyToHaggleController readyToHaggleController, IObjectPool<NpcAI> pool)
    {
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _shopSpawnPoint = shopSpawnPoint;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
        _desiredItems = desiredItems;
        _linePositionManager = linePositionManager;
        _readyToHaggleController = readyToHaggleController;
        _pool = pool;

        _agent = GetComponent<NavMeshAgent>();
        float random = UnityEngine.Random.Range(100, 250);
        _agent.speed = random / 100;
        _agent.Warp(spawnPoint);
        MoodChanged?.Invoke(MoodType.None);
        SetupTree();
        _blackboard.Set("isOutside", true);
    }
    private void Update()
    {
        if (_agent.hasPath)
        {
            FaceTarget(_agent.steeringTarget);
        }
        else
        {
            FaceTarget(_target);
        }
    }

    private void Awake()
    {
        _presenter = new NPCEmotePresenter(this, _updateEmote);
        _tree = gameObject.AddComponent<BTree>();
    }
    public void SetupTree()
    {
        _blackboard = new Blackboard();
        var root = new Selector(_blackboard, new List<BTNode>
        {
            new Sequence(_blackboard, new List<BTNode>
            {
                new CheckKeyBoolValueCondition("isOutside",true),
                new Selector(_blackboard, new List<BTNode>
                {
                    new Sequence(_blackboard, new List<BTNode>
                    {
                        new CheckKeyBoolValueCondition("isAtDoor",true),
                        new WarpAgentAction(_agent, _shopSpawnPoint),
                        new SetKeyAction<bool>("isOutside",false)
                    }),
                    new Selector(_blackboard, new List<BTNode>
                    {
                        new Sequence(_blackboard, new List<BTNode>
                        {
                            new CheckKeyBoolValueCondition("checkedWindow",true),
                            new SetMoodAction(this, MoodType.None),
                            new Selector (_blackboard, new List<BTNode>
                            {
                                new Sequence (_blackboard, new List<BTNode>
                                {
                                    new CheckKeyBoolValueCondition("shopEmpty",true),
                                    new Selector(_blackboard, new List<BTNode>
                                    {
                                        new Sequence(_blackboard, new List<BTNode>
                                        {
                                            new CheckKeyBoolValueCondition("isAtDespawnPointOutside",true),
                                            new DespawnAction(this)
                                        }),
                                        new Sequence(_blackboard, new List<BTNode>
                                        {
                                            new WalkToTargetAction(_agent,_despawnPointPos),
                                            new SetKeyAction<bool>("isAtDespawnPointOutside", true)
                                        }),
                                    })
                                }),

                                new Sequence(_blackboard, new List<BTNode>
                                {
                                    new WalkToTargetAction(_agent,_doorPos),
                                    new SetKeyAction<bool>("isAtDoor", true)
                                })
                            })
                        }),
                        new Selector(_blackboard, new List<BTNode>
                        {
                            new Sequence(_blackboard, new List<BTNode>
                            {
                                new CheckKeyBoolValueCondition("isAtWindow",true),
                                new SetMoodAction(this, MoodType.Thinking),
                                new SetLookTargetAction(this, _windowPos+new Vector3(0,0,-1)),
                                new WaitAction(GenerateRandomTime(200,500)),
                                new CheckShopHasItemsAction(),
                                new SetKeyAction<bool>("checkedWindow",true)
                            }),
                            new Sequence(_blackboard, new List<BTNode>
                                {
                                    new WalkToTargetAction(_agent,_windowPos),
                                    new SetKeyAction<bool>("isAtWindow", true)
                                })
                        })
                    })
                })
            }),
            new Selector(_blackboard, new List<BTNode>
            {
                new Sequence(_blackboard, new List<BTNode>
                {
                    new CheckKeyBoolValueCondition("isAtDespawnPointInside",true),
                    new DespawnAction(this)
                }),
                new Selector(_blackboard, new List<BTNode>
                {
                    new Sequence(_blackboard, new List<BTNode>
                    {
                        new CheckKeyBoolValueCondition("hagglingEnded",true),
                        new WalkToTargetAction(_agent, _despawnInShop),
                        new SetKeyAction<bool>("isAtDespawnPointInside", true)
                    }),
                    new Selector (_blackboard, new List<BTNode>
                    {
                        new Sequence(_blackboard, new List<BTNode>
                        {
                            new CheckKeyBoolValueCondition("hagglingStarted",true),
                            new WaitAction(0.1f),
                            new WaitUntilKeyReturnsValueAction("isHaggling",false),
                            new ReleaseFirstPositionInLineOccupancyAction(_linePositionManager),
                            new SetKeyAction<bool>("hagglingEnded",true)
                        }),
                        new Selector (_blackboard, new List<BTNode>
                        {
                            new Sequence(_blackboard, new List<BTNode>
                            {
                                new IsAtLocationCondition(_agent, _counterPos[0],0.01f),
                                new StartHagglingAction(this,this,_toleranceDecimal,_NPCType,this,_readyToHaggleController),
                                new SetLookTargetAction(this, _counterPos[0]+new Vector3(0,0,-1)),
                                new SetKeyAction<bool>("hagglingStarted",true)
                            }),
                            new Selector (_blackboard, new List<BTNode>
                            {
                                new Sequence(_blackboard, new List<BTNode>
                                {
                                    new CheckKeyBoolValueCondition("choseItemToBuy",true),
                                    new Sequence(_blackboard, new List<BTNode>
                                    {
                                        new WaitAction(0.1f),
                                        new ChoosePositionInLineAsTargetAction(_counterPos, this, _linePositionManager),
                                        new WalkToTargetAction(_agent, this),
                                        new SetLookTargetAction(this, _counterPos[0]),
                                    }),
                                }),
                                new Selector (_blackboard, new List<BTNode>
                                {
                                    new Sequence(_blackboard, new List<BTNode>
                                    {
                                        new CheckKeyBoolValueCondition("browsingStarted",true),
                                        new Selector(_blackboard, new List<BTNode>
                                        {
                                            new Sequence(_blackboard, new List<BTNode>
                                            {
                                                new CheckKeyBoolValueCondition("isAtDisplay",true),
                                                new SetMoodAction(this, MoodType.Thinking),
                                                new WaitAction(GenerateRandomTime(300,600)),
                                                new SetMoodAction(this, MoodType.None),
                                                new SetKeyAction<bool>("isAtDisplay", false),
                                                new DecidePurchaseAction(this,_desiredItems,this),
                                            }),
                                            new Selector(_blackboard, new List<BTNode>
                                            {
                                                new Sequence(_blackboard, new List<BTNode>
                                                {
                                                    new CheckKeyBoolValueCondition("hasTarget",true),
                                                    new WalkToTargetAction(_agent,this),
                                                    new SetKeyAction<bool>("hasTarget", false),
                                                    new SetKeyAction<bool>("isAtDisplay", true),
                                                }),
                                                new Sequence(_blackboard, new List<BTNode>
                                                {
                                                    new WaitAction(0.1f),
                                                    new ChooseDisplayToCheckAsTargetAction(this,this,this,this),//sets "hagglingEnded" to true if no items in list
                                                })
                                            })
                                        }),
                                    }),
                                    new Sequence(_blackboard, new List<BTNode>
                                    {
                                        new GeneratePossibleItemChoicesAction(this),
                                        new SetKeyAction<bool>("browsingStarted",true)
                                    }),
                                }),
                            }),
                        }),
                    }),
                }),
            })
        });


        _tree.Root = root;
    }
    public void Reset()
    {
        SetupTree();
    }

    #region methods
    private float GenerateRandomTime(float min, float max)
    {
        return UnityEngine.Random.Range(min, max) / 100;
    }
    public void Despawn()
    {
        Reset();
        NpcManager.Instance.DespawnNpc(this);
    }

    public void InvokeMoodChange(MoodType moodType)
    {
        MoodChanged?.Invoke(moodType);
    }
    public NPCType GetNpcType()
    {
        return _NPCType;
    }

    public void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);
        }
    }
    #endregion
}
