using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private BoxCollider2D playerHitbox;
    private Rigidbody2D rigidBody;

    private float horizontalSpeed;
    private float jumpingSpeed;

    private bool isMovingLeft;
    private bool isMovingRight;
    private float horizontalMovement;

    private Vector3 lastPositionOnNetwork;
    private float lastYSpeedOnNetwork;
    private float previousLastYSpeedOnNetwork;
    private float currentYSpeed;
    private bool jumpOnNetwork;

    private bool firstFrame;

    private bool isMovingVerticallyOnNetwork;
    private float acceleration;

    private const float TERMINAL_SPEED = -10;
    private const float ACCELERATION_BASE = -9.8f;
    private const float GRAVITY = 5f;

    private PlayerMovement()
    {
        horizontalSpeed = 7;
        jumpingSpeed = 17;

        firstFrame = true;

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

        playerHitbox = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        if (!player.PhotonView.isMine)
        {
            rigidBody.gravityScale = 0;
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
            rigidBody.velocity = new Vector2(horizontalMovement, rigidBody.velocity.y < TERMINAL_SPEED ? TERMINAL_SPEED : rigidBody.velocity.y);

            SendToServer_Movement(transform.position, rigidBody.velocity.y);
        }
        else
        {
            MovePlayerOverNetwork();
        }
    }

    private void MovePlayerOverNetwork()//still doesn't work correctly
    {
        Vector3 xMove = Vector3.MoveTowards(new Vector2(transform.position.x, 0), new Vector2(lastPositionOnNetwork.x, 0), Time.deltaTime * horizontalSpeed);
        if (lastYSpeedOnNetwork == 0)
        {
            transform.position = xMove + Vector3.MoveTowards(new Vector2(0, transform.position.y), new Vector2(0, lastPositionOnNetwork.y), Time.deltaTime * -TERMINAL_SPEED);
        }
        else
        {
            transform.position = xMove + Vector3.up * (transform.position.y + lastYSpeedOnNetwork * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime); ;
        }
    }

    private void SendToServer_Movement(Vector3 position, float ySpeed)
    {
        player.PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.Others, position, ySpeed);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement(Vector3 position, float ySpeed)
    {
        lastPositionOnNetwork = position;
        lastYSpeedOnNetwork = ySpeed;
    }

    private void OnJump()
    {
        if (!PlayerIsJumping())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
        }
    }

    private void OnMove(bool goesLeft, bool goesRight)
    {
        isMovingLeft = goesLeft;
        isMovingRight = goesRight;
    }

    private bool PlayerIsImmobile()
    {
        return rigidBody.velocity == Vector2.zero;
    }

    private bool PlayerIsJumping()
    {
        return PlayerIsMovingVertically();
    }

    private bool PlayerIsMovingHorizontally()
    {
        return rigidBody.velocity.x != 0;
    }

    private bool PlayerIsMovingVertically()
    {
        return rigidBody.velocity.y != 0;
    }
}
