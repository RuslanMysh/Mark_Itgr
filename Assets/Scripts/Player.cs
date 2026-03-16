using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    GameInput gameInput;
    [SerializeField]
    private Transform cameraTransform;




    ////////////////////////////
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float runSpeedCof = 1.6f;
    //////////////////////////////////////
    

    //.............................................................
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    //.............................................................



    private void Update()
    {

        
        HandelMovement();
        
    }

    //Решение доработано
    
    private void HandelMovement()
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
        IsRunning = gameInput.IsRunning && moveDir != Vector3.zero;


        //.............................................................
        float currentSpeed = speed; // текущая скорость
        if (IsRunning)
        {
            currentSpeed = speed * runSpeedCof; //бег
        }
        //.............................................................

        ////////////////////////////////////////
        float playerRadius = 0.7f;
        float playerHeught = 2f;
        ///////////////////////////////////////////

        float moveDistance = currentSpeed * Time.deltaTime;


        //Проверка не уперлись ли мы в объукт
        bool canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeught, playerRadius,
            moveDir, moveDistance);
        ////////////////////

        IsWalking = moveDir != Vector3.zero;

        if (!canMove)
        {
            // Попытка движения только по X относительно камеры
            Vector3 moveDirX = cameraRight * inputVector.x;


            //Проверка не уперлись ли мы в объукт
            canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeught, playerRadius,
            moveDirX, moveDistance);
            ////////////////////
            if (canMove)
            {
                moveDir = moveDirX;
            }

            else
            {
                // Попытка движения только по Z относительно камеры
                Vector3 moveDirZ = cameraForward * inputVector.y;



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
            transform.forward = Vector3.Slerp(transform.forward, moveDir, 0);

        }
    }
    
}
