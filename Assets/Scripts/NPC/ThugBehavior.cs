using UnityEngine;

public class ThugBehavior : NPCBehavior
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Player Player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [Header("Chasing")]
    [SerializeField] private float MaxChaseTime = 15f;
    [SerializeField] private float MinChaseTime = 6f;
    [SerializeField] private float ChaseRange = 10f;
    [SerializeField] private float ChaseAngle = 45f;
    [SerializeField] private float LosePlayerDelay = 0.7f;

    private float LosePlayerTimer;
    private float ChaseTime;


    private Transform playerTransform;
    enum EState
    {
        Idle,
        Wandering,
        Chasing,
        Attacking
    }

    protected override void Awake()
    {
        base.Awake();
        playerTransform = Player.transform; 
    }
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
        bool canSee = CanSeePlayer();


        if (canSee)
        {
            LosePlayerTimer = LosePlayerDelay;
        }
        else
        {
            LosePlayerTimer -= Time.deltaTime;
        }
            

        if (canSee)
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
            if (WanderTime < 0f)
            {
                ChangeState(EState.Idle);
            }                
        }
        else if (State == EState.Chasing)
        {
            NPC.Agent.SetDestination(playerTransform.position);

            ChaseTime -= Time.deltaTime;
            if (ChaseTime < 0f)
            {
                ChangeState(EState.Idle);
            }              
        }
        else if (State == EState.Attacking)
        {
            Vector3 target = playerTransform.position;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }
    protected override void OnArrived()
    {
        if (State == EState.Wandering)
        {
            ChangeState(EState.Idle);
        }           
        else if (State == EState.Chasing)
        {
            ChangeState(EState.Attacking);
        }           
    }

    void ChangeState(EState newState)
    {
        State = newState;

        if (State == EState.Wandering)
        {
            StartWandering();
        }
        else if (State == EState.Idle)
        {
            StartIdle();
        }
        else if (State == EState.Chasing)
        {
            NPC.Agent.isStopped = false;
            NPC.Agent.SetDestination(playerTransform.position);

            ChaseTime = Random.Range(MinChaseTime, MaxChaseTime);

            StartArrivalCheck();
        }
        else if (State == EState.Attacking)
        {
            NPC.Agent.isStopped = true;

            StopArrivalCheck();

            NPC.StartAttacking = true;
        }
    }

    bool CanSeePlayer()
    {
        if (Player == null)
        {
            return false;
        } 

        Vector3 origin = transform.position;
        Vector3 dir = playerTransform.position - origin;
        float sqrDistance = dir.sqrMagnitude;

        if (sqrDistance > ChaseRange * ChaseRange)
        {
            return false;
        } 

        dir.Normalize();

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > ChaseAngle)
        {
            return false;
        } 

        if (Physics.Raycast(origin, dir, out RaycastHit hit, ChaseRange, layerMask))
        {
            return hit.transform == Player.transform;
        }
            

        return false;
    }

    public void Shoot()
    {
        audioSource.PlayOneShot(shootSound);

        Vector3 origin = transform.position + transform.forward * 1f;
        Vector3 dir = (playerTransform.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, ChaseRange))
        {
            if (hit.transform == Player.transform)
            {
                Debug.Log("Попадание по игроку");
            }               
        }
    }
}