using UnityEngine;
using System.Collections;
public class Gun : MonoBehaviour
{
    [Header("Звуки")]
    [SerializeField] private AudioSource shotSound;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioSource reloadSound;
    [Header("Патроны")]
    [SerializeField] private int MaxAmmo;
    private int Ammo;
    [Header("Камера игрока")]
    [SerializeField] private Camera playerCamera;

    public int getAmmo() { return Ammo; }
    void Start()
    {
        Ammo = MaxAmmo;
        
        
        if (playerCamera == null) Debug.LogError("Камера не найдена для стрельбы!");
    }

    


    public void Shot()
    {
        // Нельзя стрелять во время перезарядки
        if (isReloading)
        {
            Debug.Log("Идет перезарядка! Подождите...");
            return;
        }
        if (playerCamera == null) return;

        if (Ammo > 0)
        {
            shotSound.PlayOneShot(shotClip);
            Ammo--;
            UIManager.Instance.AmmoT.text = Ammo.ToString();
            // Создаём луч из центра экрана (или из позиции камеры)
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            // Длина луча 50
            if (Physics.Raycast(ray, out hit, 50f))
            {
                Debug.Log($"Попадание в: {hit.collider.gameObject.name}");
                if (hit.collider == null) return;
                
                NPCBehaviorBase npc = hit.transform.GetComponentInParent<NPCBehaviorBase>();

                if (npc != null && npc.gameObject != null)
                {
                    npc.TakeDamage(10f);
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



    
    private bool isReloading = false; // Флаг перезарядки
    [Header("Перезарядка")]
    [SerializeField ]private float reloadTime = 3f; // Время перезарядки в секундах



    public void Reload()
    {
        if(Ammo != MaxAmmo)
        {
            reloadSound.Play();

            StartCoroutine(ReloadCoroutine());
            Ammo = MaxAmmo;
            UIManager.Instance.AmmoT.text = Ammo.ToString();
        }
        
    }
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        // Ждем указанное время
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

}
