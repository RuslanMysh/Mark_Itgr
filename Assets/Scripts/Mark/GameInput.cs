using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions inputActions;
    
    private bool isTake= false;
    public bool IsTake => isTake;
    

    //.............................................................
    private bool isRunning = false;
    public bool IsRunning => isRunning;
    //.............................................................
    private bool isJump = false;
    public bool IsJump => isJump;



    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Take.performed += context => isTake = true;
        inputActions.Player.NoTake.performed += context => isTake = false;
        //.............................................................
        inputActions.Player.Jump.performed += context => isJump = true;
        inputActions.Player.Jump.canceled += context => isJump = false;
        //.............................................................
        //.............................................................
        inputActions.Player.Sprint.performed += context => isRunning = true;
        inputActions.Player.Sprint.canceled += context => isRunning = false;
        //.............................................................





        inputActions.Gun.Shot.performed += Shot_performed;
    }


    


    public Vector2 GetMovementVectorNormalize()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        //GetKey - удержание клавиши
        //GetKeyDown - отпускание клавиши

        inputVector = inputVector.normalized;

        return inputVector;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    




    public event EventHandler OnShotAction;
    private void Shot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShotAction?.Invoke(this, EventArgs.Empty);
    }


}
