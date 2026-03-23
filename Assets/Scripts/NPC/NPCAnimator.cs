using UnityEngine;

public class NPCAnimator : NPCComponent
{
    private void Update()
    {
        NPC.Animator.SetFloat("Speed", NPC.CurrentSpeed);
    }
}

