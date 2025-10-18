using BehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class NPCBehaviorTree : BehaviorTree.Tree, IHasTarget, IEmotable, IMoodController, IDespawnable, IHasDisplayChoices, IHasDisplayTarget, IHasItemToBuy, IHaggler
{
    [SerializeField] private int _tolerance;
    [SerializeField] private float _toleranceDecimal;
    [SerializeField] private NPCType _NPCType;
    private NavMeshAgent _agent;
    private Transform _despawnPointPos;
    private Transform _windowPos;
    private Transform _doorPos;
    private Transform _despawnInShop;
    private Transform _shopSpawnPoint;
    [SerializeField] private List<ItemData> _desiredItems;
    private List<Vector3> _counterPos;
    private Vector3 _target;
    private DisplaySlotController _displayTarget;
    [SerializeField] private ItemData _itemToBuy;
    [SerializeField] private List<DisplaySlotController> _possibleDisplayChoices;
    private DisplaySlotController _displaySlotController;
    private Dictionary<string, Vector3> _locations;
    [SerializeField] private bool _isHaggling;

    public float ToleranceDecimal => _toleranceDecimal;
    public NavMeshAgent Agent => _agent;
    public Transform DespawnPointPos => _despawnPointPos;

    public Transform WindowPos => _windowPos;

    public Transform DoorPos => _doorPos;

    public Transform DespawnInShop => _despawnInShop;

    public Transform ShopSpawnPoint => _shopSpawnPoint;

    public List<ItemData> DesiredItems => _desiredItems;

    public Vector3 Target { get => _target; set => _target=value; }
    public DisplaySlotController DisplayTarget { get => _displayTarget; set => _displayTarget = value; }

    public List<DisplaySlotController> PossibleDisplayChoices { get => _possibleDisplayChoices; set => _possibleDisplayChoices = value; }

    public DisplaySlotController DisplaySlotController { get => _displaySlotController; set => _displaySlotController = value; }
    public ItemData ItemToBuy { get => _itemToBuy; set => _itemToBuy = value; }

    public Dictionary<string, Vector3> Locations => _locations;

    public List<Vector3> CounterPos => _counterPos;

    public bool IsHaggling { get => _isHaggling; set => _isHaggling = value; }

    public event Action Initialized;
    public event Action DecidingStarted;
    public event Action ItemRejected;
    public event Action ItemSelected;

    public event Action<MoodType> MoodChanged;

    [SerializeField] private UpdateText _updateEmote;
    private NPCEmotePresenter _presenter;

    private IObjectPool<NPCBehaviorTree> _pool;

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

    public void Initialize(bool isInShop, Vector3 spawnPoint, Transform despawnPointPos, Transform despawnInShop, Transform windowPos, Transform doorPos, Transform shopSpawnPoint, DisplaySlotController displaySlotController, List<Vector3> counterPos, List<ItemData> desiredItems, IObjectPool<NPCBehaviorTree> pool)
    {
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos;
        _doorPos = doorPos;
        _shopSpawnPoint = shopSpawnPoint;
        _displaySlotController = displaySlotController;
        _counterPos = counterPos;
        _despawnInShop = despawnInShop;
        _desiredItems = desiredItems;
        _pool = pool;

        _agent = GetComponent<NavMeshAgent>();
        float random = UnityEngine.Random.Range(100, 250);
        _agent.speed = random / 100;
        _agent.Warp(spawnPoint);
        //Initialized?.Invoke();
        MoodChanged?.Invoke(MoodType.None);
    }
    public void SetTarget(Vector3 target)
    {
        _target = target;
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
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node> {
                new Sequence(new List<Node>
                {
                    new SetTargetLeaf(this,_windowPos.position+GeneratePositionDeviation(-100,100,-10,60)),
                    new WalkToTargetLeaf(this,Agent),
                    new SetTargetLeaf(this,Target+new Vector3(0,0,-1)),
                    new WaitLeaf(GenerateRandomTime(200,500), this),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckIfShopHasItemsLeaf(),
                            new SetTargetLeaf(this,_doorPos.position),
                            new WalkToTargetLeaf(this, Agent),
                            new SetTargetLeaf(this, _shopSpawnPoint.position),
                            new WarpNode(this, Agent),
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
                                        new DecidePurchaseLeaf(this, _desiredItems,this, this),
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
                                    new SetTargetLeaf(this,_despawnInShop.position),
                                    new WalkToTargetLeaf(this, _agent),
                                    new DespawnLeaf(this),
                                })
                            }),
                        }),
                        new Sequence(new List<Node> {
                            new SetTargetLeaf(this,_despawnPointPos.position),
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
        NpcManager.Instance.DespawnNpc(this);
    }

    public void InvokeMoodChange(MoodType moodType)
    {
        MoodChanged?.Invoke(moodType);
    }
    #endregion

}
