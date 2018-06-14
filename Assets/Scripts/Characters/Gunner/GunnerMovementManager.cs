using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerMovementManager : PlayerMovementManager
{
    private bool doubleJumpAvailable;

    private GunnerMovementManager()
    {
        doubleJumpAvailable = true;
    }

    protected override void OnJump()
    {
        if (!PlayerIsMovingVertically())
        {
            isTouchingFlyingPlatformOrGround = false;
            player.EntityRigidBody.velocity = new Vector2(player.EntityRigidBody.velocity.x, jumpingSpeed);
        }
        else if (doubleJumpAvailable)
        {
            doubleJumpAvailable = false;
            player.PlayerGroundHitbox.enabled = false;
            player.EntityRigidBody.velocity = new Vector2(player.EntityRigidBody.velocity.x, jumpingSpeed);
        }
    }

    protected override void OnTouchesFlyingPlatformOrGround(GameObject platform, float objectYPosition)
    {
        base.OnTouchesFlyingPlatformOrGround(platform, objectYPosition);

        doubleJumpAvailable = true;
    }
}
