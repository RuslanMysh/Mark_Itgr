using UnityEngine;
using UnityEngine.U2D;

public class Camera : MonoBehaviour
{
    [Header("Чуствительность мыши")]
    public float sensa = 2f;
    [Header("Максимальный угол подъёма камеры")]
    public float maxYangle = 85f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    void Update()
    {
        if (PauseManager.isPaused) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        transform.parent.Rotate(Vector3.up * mouseX * sensa);

        rotationX -= mouseY*sensa;
        rotationX = Mathf.Clamp(rotationX, -maxYangle, maxYangle);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        
            
      
        

    }
}
