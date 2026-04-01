using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private int MaxAmmo;
    private int Ammo;

    [SerializeField] private Camera playerCamera;

    void Start()
    {
        Ammo = MaxAmmo;
       

        if (playerCamera == null) Debug.LogError("Камера не найдена для стрельбы!");
    }



    public void Shot()
    {
        if (playerCamera == null) return;

        if (Ammo > 0)
        {
            Ammo--;
            // Создаём луч из центра экрана (или из позиции камеры)
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            // Длина луча 50
            if (Physics.Raycast(ray, out hit, 50f))
            {
                if (hit.collider == null) return;

                ThugBehavior thug = hit.collider.GetComponentInParent<ThugBehavior>();

                if (thug != null && thug.gameObject != null)
                {
                    thug.TakeDamage(10f);
                }

            }
            else
            {
                Debug.Log("Ничего не поражено");
            }
            if (Ammo == 0)
            {
                Debug.Log("Магазин пуст! Нажмите R для перезарядки");
            }
        }
        else
        {
            Debug.Log("Нет патронов! Перезарядитесь.");
        }
    }
    public void Reload()
    {
        Ammo = MaxAmmo;
    }

}
