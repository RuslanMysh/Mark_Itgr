using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class NPCBehavior : NPCComponent
{
    public Area Area;

    [SerializeField] private LayerMask layerMask;

    [SerializeField]
    private Player Player;

    enum EState
    {
        Wandering,
        Idle,
        Chasing,
        Attacking
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

    [SerializeField]
    private float ChaseRange = 10f;
    [SerializeField]
    private float ChaseAngle = 45f;

    [SerializeField]
    private bool ChasingEnabled = false;

    [SerializeField]
    private float LosePlayerDelay = 0.7f;

    private float LosePlayerTimer;

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
        if (CanSeePlayer())
        {
            LosePlayerTimer = LosePlayerDelay;
        }
        else
        {
            LosePlayerTimer -= Time.deltaTime;
        }


        if (ChasingEnabled && CanSeePlayer())
        {
            if (State != EState.Chasing && State != EState.Attacking)
            {
                ChangeState(EState.Chasing);
            }
                
        }
        else if (State == EState.Chasing)
        {
            ChangeState(EState.Idle);
        }

        if (ChasingEnabled && CanSeePlayer() && HasArrived() && State == EState.Chasing)
        {
            ChangeState(EState.Attacking);
        }

        if (State == EState.Attacking && LosePlayerTimer <= 0f)
        {           
            NPC.EndAttacking = true;
            ChangeState(EState.Idle);           
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
        else if (State == EState.Attacking)
        {

            Vector3 target = Player.transform.position;
            Vector3 dir = Player.transform.position - transform.position;

            target.y = transform.position.y; // фикс высоты

            transform.LookAt(target);
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
        else if (State == EState.Attacking)
        {
            NPC.Agent.isStopped = true;
            NPC.StartAttacking = true;   
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
        Vector3 dir = Player.transform.position - origin;// вектор направления от NPC к игроку

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
    public void Shoot()
    {
        Vector3 origin = transform.position + transform.forward * 1f;
        Vector3 dir = (Player.transform.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, ChaseRange))
        {
            if (hit.transform == Player.transform)
            {
                Debug.Log("Попадание по игроку");
            }
        }
    }
}
