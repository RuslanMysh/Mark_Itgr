using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float time;
    void Start()
    {
        Destroy(gameObject,time);
    }

    
}
