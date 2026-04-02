using System.Collections;
using UnityEngine;

public abstract class NPCBehavior : NPCComponent
{
    public Area Area;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    [SerializeField] protected float Health = 20f;
    protected bool isDead = false;

    [Header("Waiting")]
    [SerializeField] protected float MaxWaitTime = 5f;
    [SerializeField] protected float MinWaitTime = 2f;

    [Header("Wandering")]
    [SerializeField] protected float MaxWanderTime = 15f;
    [SerializeField] protected float MinWanderTime = 6f;

    protected float WaitTime;
    protected float WanderTime;

    private Coroutine arrivalCoroutine;
    protected virtual void Awake()
    {
        base.Awake();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        SetRagdoll(false);
    }
    protected void SetRagdoll(bool state)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col.gameObject != gameObject &&
                col.GetComponent<Rigidbody>() != null)
            {
                col.enabled = state;
            }
        }

        GetComponent<Animator>().enabled = !state;
    }
    protected bool HasArrived()
    {
        if (!NPC.Agent.enabled || !NPC.Agent.isOnNavMesh)
        {
            return false;
        }
            
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

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        Health -= damage;

        if (Health <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        NPC.Agent.enabled = false;

        GetComponent<Animator>().enabled = false;
        GetComponent<Collider>().enabled = false;

        StopArrivalCheck();
        SetRagdoll(true);

        Destroy(gameObject, 10f);
    }
    protected bool IsDead()
    {
        return isDead;
    }
}