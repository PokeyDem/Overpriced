using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class NPCBehaviorTree : BehaviorTree.Tree, IHasTarget, IEmotable, IMoodController, IDespawnable, IHasDisplayChoices, IHasDisplayTarget, IHaggler
{
    [SerializeField] private float _toleranceDecimal;
    [SerializeField] private NPCType _NPCType;
    private NavMeshAgent _agent;
    private Vector3 _despawnPointPos;
    private Vector3 _windowPos;
    private Vector3 _doorPos;
    private Vector3 _despawnInShop;
    private Vector3 _shopSpawnPoint;
    private List<Vector3> _counterPos;
    private Vector3 _target;
    private List<ItemData> _desiredItems;
    private DisplaySlotController _displayTarget;
    private List<DisplaySlotController> _possibleDisplayChoices;
    private IObjectPool<NPCBehaviorTree> _pool;
    private bool _isHaggling;

    #region Properties
    public float ToleranceDecimal => _toleranceDecimal;
    public List<ItemData> DesiredItems => _desiredItems;
    public Vector3 Target { get => _target; set => _target=value; }
    public DisplaySlotController DisplayTarget { get => _displayTarget; set => _displayTarget = value; }
    public List<DisplaySlotController> PossibleDisplayChoices { get => _possibleDisplayChoices; set => _possibleDisplayChoices = value; }
    public bool IsHaggling { get => _isHaggling; set => _isHaggling = value; }
    #endregion

    #region events
    public event Action<MoodType> MoodChanged;
    #endregion

    #region 
    [SerializeField] private UpdateEmote _updateEmote;
    private NPCEmotePresenter _presenter;
    #endregion


    private void Awake()
    {
        _presenter = new NPCEmotePresenter(this, _updateEmote);
    }


    private new void Update()
    {
        if (_agent.hasPath)
        {
            FaceTarget(_agent.steeringTarget);
        }
        else 
        {
            FaceTarget(_target);
        }
        base.Update();
    }

    public void Initialize(bool isInShop, Vector3 spawnPoint, Vector3 despawnPointPos, Vector3 despawnInShop, Vector3 windowPos, Vector3 doorPos, Vector3 shopSpawnPoint, List<Vector3> counterPos, List<ItemData> desiredItems, IObjectPool<NPCBehaviorTree> pool)
    {
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _shopSpawnPoint = shopSpawnPoint;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
        _desiredItems = desiredItems;
        _pool = pool;

        _agent = GetComponent<NavMeshAgent>();
        float random = UnityEngine.Random.Range(100, 250);
        _agent.speed = random / 100;
        _agent.Warp(spawnPoint);
        MoodChanged?.Invoke(MoodType.None);
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node> {
                new Sequence(new List<Node>
                {
                    new SetTargetLeaf(this,_windowPos+GeneratePositionDeviation(-100,100,-10,60)),
                    new WalkToTargetLeaf(this,_agent),
                    new SetTargetLeaf(this,Target+new Vector3(0,0,-1)),
                    new WaitLeaf(GenerateRandomTime(200,500), this),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckIfShopHasItemsLeaf(),
                            new SetTargetLeaf(this,_doorPos),
                            new WalkToTargetLeaf(this, _agent),
                            new SetTargetLeaf(this, _shopSpawnPoint),
                            new WarpNode(this, _agent),
                            new GeneratePossibleDisplayChoicesLeaf(this),
                            new Selector(new List<Node>
                            {
                                new Sequence( new List<Node>{
                                    new LoopSequence(new List<Node>
                                    {
                                        new WaitLeaf(0.1f),
                                        new ChooseItemToCheckLeaf(this,this,this, this),
                                        new WalkToTargetLeaf(this, _agent),
                                        new WaitLeaf(GenerateRandomTime(300,600), this),
                                        new DecidePurchaseLeaf(this, _desiredItems, this),
                                    }),
                                    new LoopSequence(new List<Node>
                                    {
                                        new WaitLeaf(0.1f),
                                        new ChooseLinePositionLeaf(_counterPos,this),
                                        new WalkToTargetLeaf(this, _agent),
                                        new SetTargetLeaf(this,_counterPos[0]),
                                        new RestartIfNotFirstInLineLeaf(_agent, _counterPos[0])
                                    }),
                                    new SetTargetLeaf(this,_counterPos[0] + new Vector3(0, 0, -1)),
                                    new StartHagglingLeaf(this),
                                    new WaitUntilHagglingEndLeaf(this),
                                    new ReturnStatusLeaf(NodeState.FAILURE),
                                }),
                                new Sequence(new List<Node>
                                {
                                    new SetTargetLeaf(this,_despawnInShop),
                                    new WalkToTargetLeaf(this, _agent),
                                    new DespawnLeaf(this),
                                })
                            }),
                        }),
                        new Sequence(new List<Node> {
                            new SetTargetLeaf(this,_despawnPointPos),
                            new WalkToTargetLeaf(this, _agent),
                            new DespawnLeaf(this),
                        }),

                    }),
                }),
            }),
            new WaitLeaf(0.1f)
        });
        return root;
    }


    #region methods for Nodes
    private Vector3 GeneratePositionDeviation(float minX, float maxX, float minZ, float maxZ)
    {
        float randomX = UnityEngine.Random.Range(minX, maxX)/100;
        float randomZ = UnityEngine.Random.Range(minZ, maxZ)/100;
        return new Vector3(randomX,0, randomZ);
    }
    private float GenerateRandomTime(float min, float max)
    {
        return UnityEngine.Random.Range(min, max)/100;
    }

    #endregion
    #region methods
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
