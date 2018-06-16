using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    private Enemy enemy;

    private bool affectedByMageLeftClickIceSlow;
    private float mageLeftClickIceSlowDurationRemaining;
    private float mageLeftClickIceSlowPercent;

    private bool affectedByFighterEStun;
    private float fighterEStunDurationRemaining;

    private float jumpingSpeed;
    private bool canMove;

    private bool goLeft;
    private bool goRight;

    private bool jumpPostStun;

    private const float TERMINAL_SPEED = -10;
    private const float BASE_HORIZONTAL_SPEED = 1.5f;

    private EnemyMovementManager()
    {
        jumpingSpeed = 17;

        canMove = true;
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (canMove)
        {
            if (goRight && enemy.EntityRigidBody.velocity.x != (BASE_HORIZONTAL_SPEED * (1 - mageLeftClickIceSlowPercent)))
            {
                enemy.EntityRigidBody.velocity = new Vector2(BASE_HORIZONTAL_SPEED * (1 - mageLeftClickIceSlowPercent), enemy.EntityRigidBody.velocity.y);
            }
            else if (goLeft && enemy.EntityRigidBody.velocity.x != -BASE_HORIZONTAL_SPEED * (1 - mageLeftClickIceSlowPercent))
            {
                enemy.EntityRigidBody.velocity = new Vector2(-BASE_HORIZONTAL_SPEED * (1 - mageLeftClickIceSlowPercent), enemy.EntityRigidBody.velocity.y);
            }
        }
        else if (enemy.EntityRigidBody.velocity.x != 0)
        {
            enemy.EntityRigidBody.velocity = new Vector2(0, enemy.EntityRigidBody.velocity.y);
        }

        if (enemy.EntityRigidBody.velocity.y < TERMINAL_SPEED)
        {
            enemy.EntityRigidBody.velocity = new Vector2(enemy.EntityRigidBody.velocity.x, TERMINAL_SPEED);
        }
    }

    public void GoLeftFromTrigger()
    {
        goRight = false;
        goLeft = true;
    }

    public void GoRightFromTrigger()
    {
        goRight = true;
        goLeft = false;
    }

    public void JumpFromTrigger()
    {
        if (!EnemyIsGoingUp())
        {
            if (affectedByFighterEStun)
            {
                jumpPostStun = true;
            }
            else
            {
                enemy.EntityRigidBody.velocity = new Vector2(enemy.EntityRigidBody.velocity.x, jumpingSpeed);
            }
        }
    }

    public void StopMovementFromTrigger()
    {
        goRight = false;
        goLeft = false;
        enemy.EntityRigidBody.velocity = new Vector2(0, enemy.EntityRigidBody.velocity.y);
    }

    private bool EnemyIsGoingUp()
    {
        return enemy.EntityRigidBody.velocity.y > 0;
    }

    public void SetSlow(Ability sourceAbility, float slowDuration, float slowPercent)
    {
        if (sourceAbility is MageLeftClick)
        {
            mageLeftClickIceSlowDurationRemaining = slowDuration;
            if (!affectedByMageLeftClickIceSlow)
            {
                affectedByMageLeftClickIceSlow = true;
                mageLeftClickIceSlowPercent = slowPercent;
                StartCoroutine(Slow());
            }
        }
    }

    private IEnumerator Slow()
    {
        while (mageLeftClickIceSlowDurationRemaining > 0)
        {
            mageLeftClickIceSlowDurationRemaining -= Time.deltaTime;

            yield return null;
        }

        mageLeftClickIceSlowPercent = 0;
        affectedByMageLeftClickIceSlow = false;
    }

    public void SetStun(Ability sourceAbility, float stunDuration)
    {
        if (sourceAbility is FighterE)
        {
            fighterEStunDurationRemaining = stunDuration;
            if (!affectedByFighterEStun)
            {
                affectedByFighterEStun = true;
                canMove = false;
                StartCoroutine(Stun());
            }
        }
    }

    private IEnumerator Stun()
    {
        while (fighterEStunDurationRemaining > 0)
        {
            fighterEStunDurationRemaining -= Time.deltaTime;

            yield return null;
        }

        canMove = true;
        affectedByFighterEStun = false;
        if (jumpPostStun)
        {
            jumpPostStun = false;
            JumpFromTrigger();
        }
    }
}
