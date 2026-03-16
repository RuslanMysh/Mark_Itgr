using UnityEngine;
using UnityEngine.AI;

public class NPCComponent : MonoBehaviour
{
    protected NPC NPC;

    protected virtual void Awake()
    {
        NPC = GetComponentInParent<NPC>();
    }
}
