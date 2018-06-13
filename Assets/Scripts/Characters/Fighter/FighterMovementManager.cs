using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMovementManager : PlayerMovementManager
{
    private bool fighterQIsActive;

    private float dashSpeed;

    private FighterMovementManager()
    {
        dashSpeed = 150;
    }

    public void SetFighterQIsActive(bool active)
    {
        fighterQIsActive = active;
    }

    protected override void OnJump()
    {
        if (fighterQIsActive)
        {
            Vector3 mousePositionInsideScreen = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));
            Vector2 sceneMousePosition = StaticObjects.PlayerCamera.ScreenToWorldPoint(mousePositionInsideScreen);
            RaycastHit2D[] raycasts = Physics2D.RaycastAll(sceneMousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Structures"));
            Vector3 newPosition = sceneMousePosition;
            foreach (RaycastHit2D raycast in raycasts)
            {
                Collider2D collider = raycast.collider;
                if (collider.gameObject.tag == "Platform" || collider.gameObject.tag == "Floor")
                {
                    newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y + (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
                }
                else if (collider.gameObject.tag == "Ceiling")
                {
                    newPosition = new Vector2(newPosition.x, collider.gameObject.transform.position.y - (collider.gameObject.transform.localScale.y + transform.localScale.y) * 0.5f);
                }
                else if (collider.gameObject.tag == "Wall")
                {
                    if (transform.position.x > collider.gameObject.transform.position.x)
                    {
                        newPosition = new Vector2(collider.gameObject.transform.position.x + (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                    }
                    else
                    {
                        newPosition = new Vector2(collider.gameObject.transform.position.x - (collider.gameObject.transform.localScale.x + transform.localScale.x) * 0.5f, newPosition.y);
                    }
                }
            }

            StartCoroutine(DashToNewPosition(newPosition));
            SendToServer_FighterDash(newPosition, false);
        }
        else
        {
            base.OnJump();
        }
    }

    private void SendToServer_FighterDash(Vector3 position, bool canMove)
    {
        player.PhotonView.RPC("ReceiveFromServer_FighterDash", PhotonTargets.Others, position, canMove);
    }

    [PunRPC]
    private void ReceiveFromServer_FighterDash(Vector3 position, bool canMove)
    {
        if (!canMove)
        {
            StartCoroutine(DashToNewPosition(position));
        }
        else if (!this.canMove)
        {
            StopAllCoroutines();
            transform.position = position;
            SetCanMove(true);
        }
    }

    private IEnumerator DashToNewPosition(Vector3 newPosition)
    {
        SetCanMove(false);

        while (transform.position != newPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * dashSpeed);

            yield return null;
        }

        SetCanMove(true);
        if (player.PhotonView.isMine)
        {
            SendToServer_FighterDash(newPosition, true);
        }
    }
}
