using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPC : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent Agent;

    public Animator Animator;

    [HideInInspector]
    public bool StartAttacking = false;
    [HideInInspector]
    public bool EndAttacking = false;
    public float CurrentSpeed
    {
        get { return Agent.velocity.magnitude; }
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }
}
