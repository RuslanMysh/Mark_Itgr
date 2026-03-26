using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCWander : NPCComponent
{
    public Area Area;

    [SerializeField] private LayerMask layerMask;

    [SerializeField]
    private float ChaseRange = 10f;
    [SerializeField]
    private float ChaseAngle = 45f;

    [SerializeField]
    private bool ChasingEnabled = false;

    [SerializeField]
    private Player Player;
    enum EState
    {
        Wandering,
        Idle,
        Chasing
    }

    [Header("Waiting")]

    [SerializeField]
    private float MaxWaitTime = 5f;

    [SerializeField]
    private float MinWaitTime = 2f;

    [Header("Wandering")]

    [SerializeField]
    private float MaxWanderTime = 15f;

    [SerializeField]
    private float MinWanderTime = 6f;

    [Header("Chasing")]

    [SerializeField]
    private float MaxChaseTime = 15f;

    [SerializeField]
    private float MinChaseTime = 6f;


    [Header("DebugNPC")]

    [SerializeField]
    private float WaitTime = 10f;

    [SerializeField]
    private float WanderTime = 10f;

    [SerializeField]
    private float ChaseTime = 10f;

    [SerializeField]
    EState State = EState.Wandering;

    private void Start()
    {
        
        if (Random.Range(0f, 100f) > 50f)
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
        if (ChasingEnabled && CanSeePlayer())
        {
            if (State != EState.Chasing)
            {
                ChangeState(EState.Chasing);
            }
                
        }
        else if (State == EState.Chasing)
        {
            ChangeState(EState.Idle);
            return;
        }



        


        if (State == EState.Idle)
        {
            WaitTime -= Time.deltaTime;

            if (WaitTime < 0f)
            {
                ChangeState(EState.Wandering);
            }
        }
        else if (State == EState.Wandering)
        {
            WanderTime -= Time.deltaTime;
            if (HasArrived() || WanderTime < 0f)
            {
                ChangeState(EState.Idle);
            }
        }
        else if (State == EState.Chasing)
        {
            ChaseTime -= Time.deltaTime;
            if (HasArrived() || ChaseTime < 0f)
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

            NPC.Agent.SetDestination(Area.GetRandomPoint());

            WanderTime = Random.Range(MinWanderTime, MaxWanderTime);
        }
        else if (State == EState.Idle)
        {
            WaitTime = Random.Range(MinWaitTime, MaxWaitTime);

            NPC.Agent.isStopped = true;
        }
        else if (State == EState.Chasing)
        {
            NPC.Agent.isStopped = false;

            NPC.Agent.SetDestination(Player.transform.position);

            ChaseTime = Random.Range(MinChaseTime, MaxChaseTime);

        }

    }

    bool HasArrived()
    {
        
        if (!NPC.Agent.pathPending && NPC.Agent.remainingDistance <= NPC.Agent.stoppingDistance)//условие вычисления пути и остановки агента, если он достиг пункта назначения
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    

    bool CanSeePlayer()
    {
        if (Player == null)
        {
            return false;
        }
            
        Vector3 origin = transform.position;
        Vector3 dir = (Player.transform.position - origin);// вектор направления от NPC к игроку

        float distance = dir.magnitude;

        
        if (distance > ChaseRange)
        {
            return false;
        }
            

        dir.Normalize();

        float angle = Vector3.Angle(transform.forward, dir);//угол между направлением взгляда NPC и направлением на игрока
                                                            
        if (angle > ChaseAngle)
        {
            return false;
        }
            

        
        if (Physics.Raycast(origin, dir, out RaycastHit hit, ChaseRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform == Player.transform)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        return false;
    }

}
