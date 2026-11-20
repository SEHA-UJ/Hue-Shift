using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool isJumpPressed;
    private bool isAttackPressed;
    private bool isSwapPressed;

    private bool isLocked;
    
    private bool IsBlocked { get { return isLocked || !GameFlow.IsPlaying; } }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    public Vector2 MoveInput 
    { 
        get
        {
            if (IsBlocked)
                return Vector2.zero;

            return moveInput;
        }
    } 

    public Vector2 LookInput 
    { 
        get
        {
            if (IsBlocked)
                return Vector2.zero;

            return lookInput;
        }
    } 

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && !IsBlocked)
            isJumpPressed = true;
    }

    private void OnAttack(InputValue value)
    {
        if (value.isPressed && !IsBlocked)
            isAttackPressed = true;
    }

    private void OnSwap(InputValue value)
    {
        if (value.isPressed && !IsBlocked)
            isSwapPressed = true;
    }

    // ---------- Controller가 부르는 함수 ---------- //

    public bool ConsumeJump()
    {
        bool pressed = isJumpPressed;
        isJumpPressed = false;
        return pressed;
    }

    public bool ConsumeAttack()
    {
        bool pressed = isAttackPressed;
        isAttackPressed = false;
        return pressed;
    }

    public bool ConsumeSwap()
    {
        bool pressed = isSwapPressed;
        isSwapPressed = false;
        return pressed;
    }
}
