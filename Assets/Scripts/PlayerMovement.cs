using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;

    private float horizontalSpeed;
    private float jumpingSpeed;

    private bool isMovingLeft;
    private bool isMovingRight;
    private float horizontalMovement;

    private Vector3 lastPositionOnNetwork;
    private float lastYSpeedOnNetwork;

    private bool isMovingVerticallyOnNetwork;
    private float acceleration;

    private const float TERMINAL_SPEED = -10;
    private const float ACCELERATION_BASE = -9.8f;
    private const float GRAVITY = 5f;

    private PlayerMovement()
    {
        horizontalSpeed = 7;
        jumpingSpeed = 17;

        acceleration = ACCELERATION_BASE * GRAVITY;
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player.PlayerInputManager)
        {
            player.PlayerInputManager.OnJump += OnJump;
            player.PlayerInputManager.OnMove += OnMove;
        }
        if (player.PlayerGroundHitboxManager)
        {
            player.PlayerGroundHitboxManager.OnTouchesPlatformOrFloor += OnTouchesPlatformOrFloor;
        }
    }

    private void OnGUI()
    {
        if (player.PhotonView.isMine)
        {
            GUILayout.Label("");
            GUILayout.Label(transform.position + "");
        }
    }

    private void Update()
    {
        if (player.PhotonView.isMine)
        {
            horizontalMovement = 0;
            if (isMovingLeft)
            {
                horizontalMovement -= horizontalSpeed;
            }
            if (isMovingRight)
            {
                horizontalMovement += horizontalSpeed;
            }
            player.PlayerRigidBody.velocity = new Vector2(horizontalMovement, player.PlayerRigidBody.velocity.y < TERMINAL_SPEED ? TERMINAL_SPEED : player.PlayerRigidBody.velocity.y);

            if (player.PlayerRigidBody.velocity.y < 0 && !player.PlayerGroundHitbox.enabled)
            {
                player.PlayerGroundHitbox.enabled = true;
            }

            SendToServer_Movement(transform.position, player.PlayerRigidBody.velocity.y, PlayerIsMovingVertically());
        }
        else
        {
            MovePlayerOverNetwork();
        }
    }

    private void MovePlayerOverNetwork()
    {
        Vector3 xMove = Vector3.MoveTowards(new Vector2(transform.position.x, 0), new Vector2(lastPositionOnNetwork.x, 0), Time.deltaTime * horizontalSpeed);
        if (lastYSpeedOnNetwork == 0 && !isMovingVerticallyOnNetwork)
        {
            transform.position = xMove + Vector3.MoveTowards(new Vector2(0, transform.position.y), new Vector2(0, lastPositionOnNetwork.y), Time.deltaTime * -TERMINAL_SPEED);
        }
        else
        {
            transform.position = xMove + Vector3.up * (transform.position.y + lastYSpeedOnNetwork * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime); ;
        }
    }

    private void SendToServer_Movement(Vector3 position, float ySpeed, bool isMovingVertically)
    {
        player.PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.Others, position, ySpeed, isMovingVertically);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement(Vector3 position, float ySpeed, bool isMovingVertically)
    {
        lastPositionOnNetwork = position;
        lastYSpeedOnNetwork = ySpeed;
        isMovingVerticallyOnNetwork = isMovingVertically;
    }

    private void OnJump()
    {
        if (!PlayerIsJumping())
        {
            player.PlayerRigidBody.velocity = new Vector2(player.PlayerRigidBody.velocity.x, jumpingSpeed);
        }
    }

    private void OnMove(bool goesLeft, bool goesRight)
    {
        isMovingLeft = goesLeft;
        isMovingRight = goesRight;
    }

    private void OnTouchesPlatformOrFloor(float objectYPosition)
    {
        player.PlayerGroundHitbox.enabled = false;
        player.PlayerRigidBody.velocity = new Vector2(player.PlayerRigidBody.velocity.x, 0);
        transform.position = new Vector3(transform.position.x, objectYPosition + (transform.localScale.y * 0.5f));
    }

    private bool PlayerIsImmobile()
    {
        return player.PlayerRigidBody.velocity == Vector2.zero;
    }

    private bool PlayerIsJumping()
    {
        return PlayerIsMovingVertically();
    }

    private bool PlayerIsMovingHorizontally()
    {
        return player.PlayerRigidBody.velocity.x != 0;
    }

    private bool PlayerIsMovingVertically()
    {
        return player.PlayerRigidBody.velocity.y > 0 || player.PlayerGroundHitbox.enabled;
    }
}
