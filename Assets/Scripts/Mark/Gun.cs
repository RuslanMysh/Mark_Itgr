using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private int MaxAmmo;
    private int Ammo;
    
    void Start()
    {
        Ammo = MaxAmmo;
    }

    public void Shot()
    {
        Debug.Log("Ïèó");
    }
    
}
