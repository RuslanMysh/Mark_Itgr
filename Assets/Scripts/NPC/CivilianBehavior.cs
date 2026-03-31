using UnityEngine;

public class CivilianBehavior : NPCBehavior
{
    enum EState
    {
        Idle,
        Wandering
    }

    [SerializeField] EState State = EState.Wandering;

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
    }
    protected override void OnArrived()
    {
        if (State == EState.Wandering)
        {
            ChangeState(EState.Idle);
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
    }
}