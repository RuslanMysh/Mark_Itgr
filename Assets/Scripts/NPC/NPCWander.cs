using UnityEngine;
using UnityEngine.AI;

public class NPCWander : NPCComponent
{
    public Area Area;

    enum EState
    {
        Wandering,
        Idle
    }
    [SerializeField]
    float MaxWaitTime = 3f;

    [SerializeField]
    float MaxWaitTimeRandom = 5f;

    [SerializeField]
    float maxWanderTime = 5f;



    float currentMaxWaitTime = 3f;

    [Header("DebugNPC")]

    [SerializeField]
    private float waitTime = 0f;

    [SerializeField]
    private float wanderTime = 0f;

    [SerializeField]
    EState State = EState.Wandering;

    private void Start()
    {
        if (Random.Range(0f, 100.0f) > 50f)
        {
            ChangeState(EState.Wandering);
        }
        else
        {
            ChangeState(EState.Idle);
        }
    }

    private void Update()
    {
        if(State == EState.Idle)
        {
            waitTime -= Time.deltaTime;

            if (waitTime < 0f)
            {
                ChangeState(EState.Wandering);
            }
        }
        else if (State == EState.Wandering)
        {
            wanderTime -= Time.deltaTime;
            if (HasArrived() || wanderTime < 0f)
            {
                ChangeState(EState.Idle);
            }
        }
    }

    void ChangeState(EState newState)
    {
        State = newState;

        if (State == EState.Wandering)
        {
            NPC.Agent.isStopped = false;
            SetRandomDestination();       
            
            wanderTime = maxWanderTime;
        }
        else if (State == EState.Idle)
        {
            waitTime = MaxWaitTime + Random.Range(0f, MaxWaitTimeRandom);

            NPC.Agent.isStopped = true;
        }
    }

    bool HasArrived()
    {
        return NPC.Agent.remainingDistance <= NPC.Agent.stoppingDistance;
    }

    void SetRandomDestination()
    {
        NPC.Agent.SetDestination(Area.GetRandomPoint());
    }
}
