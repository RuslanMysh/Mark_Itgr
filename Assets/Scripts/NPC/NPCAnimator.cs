using UnityEngine;

public class NPCAnimator : NPCComponent
{
    private const string START_ATTACKING = "StartAttacking";
    private const string END_ATTACKING = "EndAttacking";
    private void Awake()
    {
        base.Awake();
        NPC.Animator.SetBool(START_ATTACKING, false);
        NPC.Animator.SetBool(END_ATTACKING, false);
    }
    private void Update()
    {
        NPC.Animator.SetFloat("Speed", NPC.CurrentSpeed);
        if (NPC.StartAttacking)
        {
            NPC.Animator.SetTrigger(START_ATTACKING);
            NPC.StartAttacking = false;
        }
        if (NPC.EndAttacking)
        {
            NPC.Animator.SetTrigger(END_ATTACKING);
            NPC.EndAttacking = false;
        }
    }
}

