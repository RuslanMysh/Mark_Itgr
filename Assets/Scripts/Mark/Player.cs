using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;

    [Header("Характеристики передвижения")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float runSpeedCof = 1.6f;
    [SerializeField] private float jumpForce = 5f;

    private float gravity = -9.8f;
    private Vector3 playerVelocity;

    public bool IsTake { get; private set; }
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }

    // Параметры для капсулы
    private float playerRadius;
    private float playerHeight;

    [Header("Пистолет игрока")]
    [SerializeField] private GameObject visualGameObject;
    [SerializeField] private Gun gun;



    [Header("Параметры Выносливости")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 15f;    // восстановление в секунду
    [SerializeField] private float runStaminaCost = 20f;      // расход в секунду при беге
    [SerializeField] private float jumpStaminaCost = 25f;     // расход за прыжок
    private float currentStamina;
    private Vector3 lastMoveDir;



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

        currentStamina = maxStamina;

        gameInput.OnShot += GameInput_OnShot;
        gameInput.OnReload += GameInput_OnReload;
        
    }

    private void Update()
    {
        HandleMovement();
        TakeGun();
        
        
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

        lastMoveDir = moveDir;

        // Определяем режим движения
        IsRunning = gameInput.IsRunning && moveDir != Vector3.zero;
        float currentSpeed = IsRunning ? speed * runSpeedCof : speed;

        //отключаем бег, если выносливость кончилась 
        if (IsRunning && currentStamina <= 0f)
        {
            currentSpeed = speed;
        }
        
        IsWalking = moveDir != Vector3.zero;

        // Горизонтальное движение
        Vector3 horizontalMove = moveDir * currentSpeed * Time.deltaTime;

        // Вертикальное движение (гравитация и прыжок)
        if (characterController.isGrounded)
        {
            playerVelocity.y = -0.5f; // Небольшое прижатие к земле

            if (gameInput.IsJump) // Используем GameInput для прыжка
            {
                if (currentStamina >= jumpStaminaCost)
                {
                    playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    currentStamina -= jumpStaminaCost;
                }
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
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        // Бег активен, если зажата клавиша бега, есть движение и выносливость > 0
        bool isRunningActive = IsRunning && currentStamina > 0f;

        if (isRunningActive)
        {
            // Расход выносливости
            currentStamina -= runStaminaCost * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
        }
        else if (characterController.isGrounded)
        {
            // Восстановление
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }
    }

    private void TakeGun()
    {
        IsTake = gameInput.IsTake;
        if (IsTake)
        {
            Show();
        }
        else
        {
            Hide();
        }


    }
    private void Show()
    {
        visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }

    private void GameInput_OnShot(object sender, EventArgs e)
    {
        if (visualGameObject.activeSelf) // Проверяем, взят ли пистолет
        {
            gun?.Shot();
        }
    }

    private void GameInput_OnReload(object sender, EventArgs e)
    {
        if (visualGameObject.activeSelf)
        {
           gun?.Reload();
        }
    }





    [Header("Хп персонажа")]
    [SerializeField] private float health = 50f;
    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log("Player HP: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("Вы умерли");

       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}