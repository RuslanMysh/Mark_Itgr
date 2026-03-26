using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float runSpeedCof = 1.6f;
    [SerializeField] private float jumpForce = 5f;

    private float gravity = -9.8f;
    private Vector3 playerVelocity;

    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }

    // Параметры для капсулы
    private float playerRadius;
    private float playerHeight;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController компонент не найден!");
        }

        // Используем параметры из CharacterController
        playerRadius = characterController.radius;
        playerHeight = characterController.height;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalize();

        // Берем forward камеры, но обнуляем Y и нормализуем для движения по горизонтали
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Берем right камеры, тоже обнуляем Y
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Создаем вектор движения относительно камеры
        Vector3 moveDir = cameraForward * inputVector.y + cameraRight * inputVector.x;
        moveDir.Normalize();

        // Определяем режим движения
        IsRunning = gameInput.IsRunning && moveDir != Vector3.zero;
        float currentSpeed = IsRunning ? speed * runSpeedCof : speed;
        IsWalking = moveDir != Vector3.zero;

        // Горизонтальное движение
        Vector3 horizontalMove = moveDir * currentSpeed * Time.deltaTime;

        // Вертикальное движение (гравитация и прыжок)
        if (characterController.isGrounded)
        {
            playerVelocity.y = -0.5f; // Небольшое прижатие к земле

            if (gameInput.IsJump) // Используем GameInput для прыжка
            {
                playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        // Объединяем горизонтальное и вертикальное движение
        Vector3 finalMove = horizontalMove + playerVelocity * Time.deltaTime;

        // Двигаем персонажа через CharacterController
        characterController.Move(finalMove);

        // Поворачиваем персонажа в направлении движения
        if (moveDir != Vector3.zero && characterController.isGrounded)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, 0);
        }
    }
}