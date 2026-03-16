using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions inputActions;

    //.............................................................
    private bool isRunning = false;
    public bool IsRunning => isRunning;
    //.............................................................

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

       

        //.............................................................
        inputActions.Player.Sprint.performed += context => isRunning = true;
        inputActions.Player.Sprint.canceled += context => isRunning = false;
        //.............................................................
    }


    public Vector2 GetMovementVectorNormalize()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        //GetKey - удержание клавиши
        //GetKeyDown - отпускание клавиши

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
