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
    private Vector3 lastImmobilePosition;

    private bool waitForImmobility;

    private const float TERMINAL_SPEED = -18;

    private PlayerMovement()
    {
        horizontalSpeed = 7;
        jumpingSpeed = 17;

        lastImmobilePosition = Vector3.zero;
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
        if (waitForImmobility && PlayerIsImmobile())
        {
            waitForImmobility = false;
            SendToServer_Movement(MovementType.RUBBERBAND, transform.position);
        }

        if (PlayerIsMovingVertically() && rigidBody.velocity.y < TERMINAL_SPEED)
        {
            rigidBody.velocity += Vector2.up * (TERMINAL_SPEED - rigidBody.velocity.y);
        }

        if (isMovingLeft || isMovingRight)
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
            rigidBody.velocity = new Vector2(horizontalMovement, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            if (lastImmobilePosition != Vector3.zero && PlayerIsImmobile() && lastImmobilePosition != transform.position)
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(lastImmobilePosition.x, transform.position.y), Time.deltaTime * horizontalSpeed);
            }
            else if (lastImmobilePosition == transform.position)
            {
                lastImmobilePosition = Vector3.zero;
            }
        }
    }

    private void SendToServer_Movement(MovementType movementType, Vector3 position, bool goesLeft = false, bool goesRight = false)
    {
        player.PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.Others, movementType, position, goesLeft, goesRight);
    }

    [PunRPC]
    private void ReceiveFromServer_Movement(MovementType movementType, Vector3 position, bool goesLeft, bool goesRight)
    {
        switch (movementType)
        {
            case MovementType.JUMP:
                Jump(position);
                break;
            case MovementType.MOVE:
                Move(position, goesLeft, goesRight);
                break;
            case MovementType.RUBBERBAND:
                lastImmobilePosition = position;
                break;
        }
    }

    private void OnJump()
    {
        if (!PlayerIsJumping())
        {
            SendToServer_Movement(MovementType.JUMP, transform.position);
            Jump(transform.position);
        }
    }

    private void Jump(Vector3 position)
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, Vector3.up.y * jumpingSpeed);
    }

    private void OnMove(bool goesLeft, bool goesRight)
    {
        if (isMovingLeft != goesLeft || isMovingRight != goesRight)
        {
            SendToServer_Movement(MovementType.MOVE, transform.position, goesLeft, goesRight);
            Move(transform.position, goesLeft, goesRight);
        }
    }

    private void Move(Vector3 position, bool goesLeft, bool goesRight)
    {
        isMovingLeft = goesLeft;
        isMovingRight = goesRight;
        if (player.PhotonView.isMine && !isMovingLeft && !isMovingRight)
        {
            waitForImmobility = true;
        }
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

public enum MovementType
{
    JUMP,
    MOVE,
    RUBBERBAND
}
