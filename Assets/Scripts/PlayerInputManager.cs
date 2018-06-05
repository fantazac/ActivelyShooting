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

    public delegate void OnLeftClickHandler(Vector3 mousePosition);
    public event OnLeftClickHandler OnLeftClick;

    public delegate void OnRightClickHandler(Vector3 mousePosition);
    public event OnRightClickHandler OnRightClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && OnLeftClick != null)
        {
            OnLeftClick(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1) && OnRightClick != null)
        {
            OnRightClick(Input.mousePosition);
        }

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
