using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    protected Player player;

    protected float horizontalSpeed;
    protected float jumpingSpeed;

    protected bool isMovingLeft;
    protected bool isMovingRight;
    protected float horizontalMovement;

    protected Vector3 lastPositionOnNetwork;
    protected float lastYSpeedOnNetwork;

    protected bool isMovingVerticallyOnNetwork;
    protected float acceleration;

    protected GameObject flyingPlatform;

    protected bool isTouchingFlyingPlatformOrGround;
    protected bool canJump;
    protected bool canMove;

    protected float jumpDownDuration;
    protected WaitForSeconds jumpDownDelay;

    protected const float TERMINAL_SPEED = -10;
    protected const float ACCELERATION_BASE = -9.8f;
    protected const float GRAVITY = 4f;
    protected const float BASE_HORIZONTAL_SPEED = 7;

    protected PlayerMovementManager()
    {
        canJump = true;
        canMove = true;

        horizontalSpeed = BASE_HORIZONTAL_SPEED;
        jumpingSpeed = 17;

        acceleration = ACCELERATION_BASE * GRAVITY;

        jumpDownDuration = 0.3f;
        jumpDownDelay = new WaitForSeconds(jumpDownDuration);
    }

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
        if (player.PlayerInputManager)
        {
            player.PlayerInputManager.OnJump += OnJump;
            player.PlayerInputManager.OnJumpDown += OnJumpDown;
            player.PlayerInputManager.OnMove += OnMove;
        }
        if (player.PlayerGroundHitboxManager)
        {
            player.PlayerGroundHitboxManager.OnTouchesFlyingPlatformOrGround += OnTouchesFlyingPlatformOrGround;
        }
    }

    protected void OnGUI()
    {
        if (player.PhotonView.isMine)
        {
            GUILayout.Label("");//Ping
            GUILayout.Label(transform.position + "");
        }
    }

    public void ChangeHorizontalSpeed(float percent)
    {
        horizontalSpeed = BASE_HORIZONTAL_SPEED * percent;
    }

    public void SetCanJump(bool canJump)
    {
        this.canJump = canJump;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        if (player.PhotonView.isMine)
        {
            if (canMove)
            {
                player.EntityRigidBody.gravityScale = GRAVITY;
            }
            else
            {
                player.EntityRigidBody.gravityScale = 0;
                player.EntityRigidBody.velocity = Vector2.zero;
            }
        }
    }

    protected void Update()
    {
        if (player.PhotonView.isMine)
        {
            if (canMove)
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
                player.EntityRigidBody.velocity = new Vector2(horizontalMovement, player.EntityRigidBody.velocity.y < TERMINAL_SPEED ? TERMINAL_SPEED : player.EntityRigidBody.velocity.y);

                if (player.EntityRigidBody.velocity.y < 0 && !player.PlayerGroundHitbox.enabled)
                {
                    isTouchingFlyingPlatformOrGround = false;
                    player.PlayerGroundHitbox.enabled = true;
                }
                if (PlayerIsMovingVertically())
                {
                    flyingPlatform = null;
                }
            }

            SendToServer_Movement(transform.position, player.EntityRigidBody.velocity.y, PlayerIsMovingVertically());
        }
        else
        {
            MovePlayerOverNetwork();
        }
    }

    protected void MovePlayerOverNetwork()
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

    protected void SendToServer_Movement(Vector3 position, float ySpeed, bool isMovingVertically)
    {
        player.PhotonView.RPC("ReceiveFromServer_Movement", PhotonTargets.Others, position, ySpeed, isMovingVertically);
    }

    [PunRPC]
    protected void ReceiveFromServer_Movement(Vector3 position, float ySpeed, bool isMovingVertically)
    {
        lastPositionOnNetwork = position;
        lastYSpeedOnNetwork = ySpeed;
        isMovingVerticallyOnNetwork = isMovingVertically;
    }

    protected virtual void OnJump()
    {
        if (!PlayerIsMovingVertically() && canJump && canMove)
        {
            isTouchingFlyingPlatformOrGround = false;
            player.EntityRigidBody.velocity = new Vector2(player.EntityRigidBody.velocity.x, jumpingSpeed);
        }
    }

    protected void OnMove(bool goesLeft, bool goesRight)
    {
        isMovingLeft = goesLeft;
        isMovingRight = goesRight;
    }

    protected void OnJumpDown()
    {
        if (flyingPlatform && canJump && canMove)
        {
            flyingPlatform = null;
            player.PlayerHitbox.isTrigger = true;
            StartCoroutine(EnablePlayerHitbox());
        }
    }

    protected IEnumerator EnablePlayerHitbox()
    {
        yield return jumpDownDelay;

        //hitbox.enabled = true;
        player.PlayerHitbox.isTrigger = false;
    }

    protected virtual void OnTouchesFlyingPlatformOrGround(GameObject flyingPlatform, float objectYPosition)
    {
        if (!player.PlayerHitbox.isTrigger)
        {
            player.PlayerGroundHitbox.enabled = false;
            isTouchingFlyingPlatformOrGround = true;
            if (flyingPlatform)
            {
                this.flyingPlatform = flyingPlatform;
            }
            transform.position = new Vector3(transform.position.x, objectYPosition + (transform.localScale.y * 0.5f));
            player.EntityRigidBody.velocity = new Vector2(player.EntityRigidBody.velocity.x, 0);
            transform.position = new Vector3(transform.position.x, objectYPosition + (transform.localScale.y * 0.5f));
            player.EntityRigidBody.velocity = new Vector2(player.EntityRigidBody.velocity.x, 0);
        }
    }

    protected bool PlayerIsImmobile()
    {
        return player.EntityRigidBody.velocity == Vector2.zero;
    }

    protected bool PlayerIsMovingHorizontally()
    {
        return player.EntityRigidBody.velocity.x != 0;
    }

    public bool PlayerIsMovingVertically()
    {
        return !isTouchingFlyingPlatformOrGround;
    }
}
