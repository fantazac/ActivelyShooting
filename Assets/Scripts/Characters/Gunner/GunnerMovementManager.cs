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
            isTouchingFloorOrPlatform = false;
            player.PlayerRigidBody.velocity = new Vector2(player.PlayerRigidBody.velocity.x, jumpingSpeed);
        }
        else if (doubleJumpAvailable)
        {
            doubleJumpAvailable = false;
            player.PlayerGroundHitbox.enabled = false;
            player.PlayerRigidBody.velocity = new Vector2(player.PlayerRigidBody.velocity.x, jumpingSpeed);
        }
    }

    protected override void OnTouchesPlatformOrFloor(GameObject platform, float objectYPosition)
    {
        base.OnTouchesPlatformOrFloor(platform, objectYPosition);

        doubleJumpAvailable = true;
    }
}
