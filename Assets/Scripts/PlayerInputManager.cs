using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public delegate void OnMoveUpWHandler();
    public event OnMoveUpWHandler OnMoveUp;//Not used unless we have portals to change maps

    public delegate void OnMoveRightHandler(bool goesLeft, bool goesRight);
    public event OnMoveRightHandler OnMove;

    public delegate void OnJumpHandler();
    public event OnJumpHandler OnJump;

    public delegate void OnJumpDownHandler();
    public event OnJumpDownHandler OnJumpDown;

    public delegate void OnAbilityPressedHandler(int abilityId, Vector3 mousePosition, bool isPressed = false);
    public event OnAbilityPressedHandler OnAbilityPressed;

    public delegate void OnTabPressedHandler();
    public event OnTabPressedHandler OnTabPressed;

    private void Update()
    {
        //Abilities + Attacks
        if (Input.GetMouseButton(0))
        {
            OnAbilityPressed(2, Input.mousePosition, true);
        }
        else
        {
            OnAbilityPressed(2, Input.mousePosition, false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnAbilityPressed(3, Input.mousePosition, true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnAbilityPressed(3, Input.mousePosition, false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnAbilityPressed(0, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnAbilityPressed(1, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnTabPressed();
        }

        //Movement
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnJumpDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }
        OnMove(Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D));
    }
}
