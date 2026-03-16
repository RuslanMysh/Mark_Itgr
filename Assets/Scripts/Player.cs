using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    GameInput gameInput;
    
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float runSpeedCof = 1.6f;


    [SerializeField]
    private float rotationSpeed = 10f;

    private Vector3 lastInteractionDir;

    public bool IsWalking { get; private set; }

    //.............................................................
    public bool IsRunning { get; private set; }
    //.............................................................

    


    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (Instance == null)
        {
            Debug.LogError("Ќа уровне больше 1 игрока");
        }


    }
    private void Update()
    {
        HandelMovement();
        
    }


    private void HandelMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalize();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        IsRunning = gameInput.IsRunning && moveDir != Vector3.zero;

        //.............................................................
        float currentSpeed = speed; // текуща€ скорость
        if (IsRunning)
        {
            currentSpeed = speed * runSpeedCof; //бег
        }
        //.............................................................


        float playerRadius = 0.7f;
        float playerHeught = 2f;

        float moveDistance = currentSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeught, playerRadius,
            moveDir, moveDistance);

        IsWalking = moveDir != Vector3.zero;

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeught, playerRadius,
            moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeught, playerRadius,
                moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }

            }

        }


        if (canMove)
        {
            transform.position += currentSpeed * moveDir * Time.deltaTime;
        }

        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);

        }
    }
}
