using System.Collections;
using UnityEngine;

public abstract class NPCBehavior : NPCComponent
{
    public Area Area;

    [Header("Waiting")]
    [SerializeField] protected float MaxWaitTime = 5f;
    [SerializeField] protected float MinWaitTime = 2f;

    [Header("Wandering")]
    [SerializeField] protected float MaxWanderTime = 15f;
    [SerializeField] protected float MinWanderTime = 6f;

    protected float WaitTime;
    protected float WanderTime;

    private Coroutine arrivalCoroutine;

    protected bool HasArrived()
    {
        return !NPC.Agent.pathPending && NPC.Agent.remainingDistance <= NPC.Agent.stoppingDistance;

    }
    protected void StartArrivalCheck()
    {
        StopArrivalCheck();
        arrivalCoroutine = StartCoroutine(ArrivalRoutine());
    }
    protected void StopArrivalCheck()
    {
        if (arrivalCoroutine != null)
        {
            StopCoroutine(arrivalCoroutine);
            arrivalCoroutine = null;
        }
    }
    private IEnumerator ArrivalRoutine()
    {
        var wait = new WaitForSeconds(0.1f); 
        while (true)
        {
            yield return wait;
            if (HasArrived())
            {
                OnArrived();
            } 
        }
    }
    protected abstract void OnArrived();
    protected void StartWandering()
    {
        NPC.Agent.isStopped = false;
        NPC.Agent.SetDestination(Area.GetRandomPoint());
        WanderTime = Random.Range(MinWanderTime, MaxWanderTime);
        StartArrivalCheck();
    }

    protected void StartIdle()
    {
        NPC.Agent.isStopped = true;
        WaitTime = Random.Range(MinWaitTime, MaxWaitTime);
        StopArrivalCheck();
    }
}