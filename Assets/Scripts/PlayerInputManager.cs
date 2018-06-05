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

    public delegate void OnLeftClickHandler(Vector3 mousePosition, bool isPressed);
    public event OnLeftClickHandler OnLeftClick;

    public delegate void OnRightClickHandler(Vector3 mousePosition);
    public event OnRightClickHandler OnRightClick;

    public delegate void OnQPressedHandler();
    public event OnQPressedHandler OnQPressed;

    public delegate void OnEPressedHandler();
    public event OnEPressedHandler OnEPressed;

    public delegate void OnTabPressedHandler();
    public event OnTabPressedHandler OnTabPressed;

    private void Update()
    {
        //Abilities + Attacks
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick(Input.mousePosition, true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnLeftClick(Input.mousePosition, false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick(Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQPressed();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnEPressed();
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
