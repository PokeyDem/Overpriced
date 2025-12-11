
using BehaviorTreeTest;
using GOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public enum AgentState 
{
    Outside,
    Inside,

}


[RequireComponent(typeof(NavMeshAgent))]
public class GoapAgent : MonoBehaviour, IHasTarget, IEmotable, IMoodController, IDespawnable, IHasDisplayChoices, IHasDisplayTarget, IHaggler
{
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
    private IObjectPool<GoapAgent> _pool;
    private bool _isHaggling;
    #endregion

    ILinePositionManager _linePositionManager;
    INPCReadyToHaggleController _readyToHaggleController;

    #region Properties
    public Vector3 Target { get => _target; set => _target = value; }
    public List<DisplayContext> PossibleDisplayChoices { get => _possibleDisplayChoices; set => _possibleDisplayChoices = value; }
    public DisplayContext DisplayTarget { get => _displayTarget; set => _displayTarget = value; }
    public bool IsHaggling { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    #endregion

    #region events
    public event Action<MoodType> MoodChanged;
    #endregion

    #region UI stuff
    [SerializeField] private UpdateEmote _updateEmote;
    private NPCEmotePresenter _presenter;
    #endregion

    #region GOAP stuff
    private AgentGoal _lastGoal;
    public AgentGoal currentGoal;
    public ActionPlan actionPlan;
    public AgentAction currentAction;
    public CountdownTimer statsTimer;

    public Dictionary<string, AgentBelief> beliefs;
    public HashSet<AgentAction> actions;
    public HashSet<AgentGoal> goals;

    private IGoapPlanner _gPlanner;
    #endregion

    #region GOAP states
    private bool _waitedAtWindow=false;
    private bool _shopEmpty=true;
    #endregion

    private void Awake()
    {
        _presenter = new NPCEmotePresenter(this, _updateEmote);
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
        if (currentAction==null)
        {
            Debug.Log("Calculating any potential new plan");
            CalculatePlan();

            if (actionPlan != null && actionPlan.Actions.Count > 0) 
            {
                _agent.ResetPath();

                currentGoal = actionPlan.AgentGoal;
                currentAction = actionPlan.Actions.Pop();
                currentAction.Start();
                Debug.Log($"Goal: {currentGoal.Name} with {actionPlan.Actions.Count} actions in plan");
                Debug.Log($"Popped action: {currentAction.Name}");
            }
        }

        if(actionPlan!=null && currentAction!=null)
        {
            currentAction.Update(Time.deltaTime);

            if(currentAction.Complete)
            {
                Debug.Log($"{currentAction.Name} complete");
                currentAction.Stop();
                currentAction = null;

                if (actionPlan.Actions.Count == 0)
                {
                    Debug.Log("Plan complete");
                    _lastGoal = currentGoal;
                    currentGoal = null;
                }
            }
        }
    }

    public void Initialize(bool isInShop, Vector3 spawnPoint, Vector3 despawnPointPos, Vector3 despawnInShop, Vector3 windowPos, Vector3 doorPos, Vector3 shopSpawnPoint, List<Vector3> counterPos, List<ItemData> desiredItems, ILinePositionManager linePositionManager, INPCReadyToHaggleController readyToHaggleController, IObjectPool<GoapAgent> pool)
    {
        _despawnPointPos = despawnPointPos;
        _windowPos = windowPos+GeneratePositionDeviation(-100, 100, -10, 60);
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
        _gPlanner = new GoapPlanner();
        SetupTimers();
        SetupBeliefs();
        SetupActions();
        SetupGoals();
    }

    #region GOAP methods
    private void CalculatePlan()
    {
        var priorityLevel = currentGoal?.Priority ?? 0;

        HashSet<AgentGoal> goalsToCheck = goals;

        if (currentGoal != null)
        {
            Debug.Log("Current goal exists, checking goals with higher priority");
            goalsToCheck = new HashSet<AgentGoal>(goals.Where(g => g.Priority > priorityLevel));
        }

        var potentialPlan = _gPlanner.Plan(this, goalsToCheck, _lastGoal);
        if(potentialPlan!=null)
        {
            actionPlan =potentialPlan;
        }
    }
    private void SetupBeliefs()
    {
        beliefs = new Dictionary<string, AgentBelief>();
        BeliefFactory factory = new BeliefFactory(this, beliefs);

        factory.AddBelief("Nothing", () => false);

        factory.AddBelief("AgentIdle", () => !_agent.hasPath);
        factory.AddBelief("AgentMoving", () => _agent.hasPath);

        factory.AddLocationBelief("AtWindow", 2f, _windowPos);
        factory.AddLocationBelief("AtDoor", 2f, _doorPos);
        factory.AddLocationBelief("AtDespawnOutside", 2f, _doorPos);

        factory.AddBelief("ShopEmpty", () => _shopEmpty);//DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count == 0
        factory.AddBelief("ShopNotEmpty", () => !_shopEmpty);
        factory.AddBelief("WaitedAtWindow", () => _waitedAtWindow);
    }
    private void SetupActions()
    {
        actions = new HashSet<AgentAction>();
        actions.Add(new AgentAction.Builder("WalkToWindow")
            .WithStrategy(new MoveStrategy(_agent,() => _windowPos))
            .AddEffect(beliefs["AtWindow"])
            .Build());
        actions.Add(new AgentAction.Builder("WaitAtWindow")
            .WithStrategy(new CompositeStrategy(new IActionStrategy[]
            {
                new IdleStrategy(5),
                new SetBoolStrategy(v=>_waitedAtWindow=v, ()=>true),
                new SetBoolStrategy(v=>_shopEmpty=v, ()=>DisplaysWithItemsListHandler.Instance.GetDisplaySlotsWithItems().Count == 0),
            }))
            .AddPrecondition(beliefs["AtWindow"])
            .AddEffect(beliefs["WaitedAtWindow"])
            //.AddEffect(beliefs["ShopNotEmpty"])
            //.AddEffect(beliefs["ShopEmpty"])
            .Build());

        actions.Add(new AgentAction.Builder("WalkToDoor")
            .WithStrategy(new MoveStrategy(_agent,()=>_doorPos))
            .AddPrecondition(beliefs["WaitedAtWindow"])
            .AddPrecondition(beliefs["ShopNotEmpty"])
            .AddEffect(beliefs["AtDoor"])
            .Build());
        actions.Add(new AgentAction.Builder("WalkToDespawnOutside")
            .WithStrategy(new CompositeStrategy(new IActionStrategy[]
            {
                new MoveStrategy(_agent,()=>_despawnPointPos),
                new DespawnStrategy(this)
            }))
            .AddPrecondition(beliefs["WaitedAtWindow"])
            .AddPrecondition(beliefs["ShopEmpty"])
            .AddEffect(beliefs["AtDespawnOutside"])
            .Build());
    }
    private void SetupGoals()
    {
        goals =  new HashSet<AgentGoal>();

        goals.Add(new AgentGoal.Builder("DailyRoutine")
            .WithPriority(2)
            .WithDesiredEffect(beliefs["AtDoor"])
            .Build());
        goals.Add(new AgentGoal.Builder("DailyRoutine2")
            .WithPriority(1)
            .WithDesiredEffect(beliefs["AtDespawnOutside"])
            .Build());
    }


    private void SetupTimers()
    {
        statsTimer = new CountdownTimer(2f);
        statsTimer.OnTimerStop += () =>
        {
            //UpdateStats();
            statsTimer.Start();
        };
        statsTimer.Start();
    }

    //private void UpdateStats()
    //{
    //    stamina += InRangeOf(restingPosition.position, 3f) ? 20 : -10;
    //    health += InRangeOf(foodShack.position, 3f) ? 20 : -5;
    //    stamina = Mathf.Clamp(stamina, 0, 100);
    //    health = Mathf.Clamp(health, 0, 100);
    //}

    bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(transform.position,pos) < range;
    #endregion

    #region methods
    private float GenerateRandomTime(float min, float max)
    {
        return UnityEngine.Random.Range(min, max) / 100;
    }
    public void Despawn()
    {
        //NpcManager.Instance.DespawnNpc(this);
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
    private Vector3 GeneratePositionDeviation(float minX, float maxX, float minZ, float maxZ)
    {
        float randomX = UnityEngine.Random.Range(minX, maxX) / 100;
        float randomZ = UnityEngine.Random.Range(minZ, maxZ) / 100;
        return new Vector3(randomX, 0, randomZ);
    }
    #endregion
}
