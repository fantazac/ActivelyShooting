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

    private float movementSpeed;
    private bool canMove;

    private EnemyMovementManager()
    {
        movementSpeed = 1;
        canMove = true;
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (canMove && enemy.EntityRigidBody.velocity.x != movementSpeed * (1 - mageLeftClickIceSlowPercent))
        {
            enemy.EntityRigidBody.velocity = new Vector2(movementSpeed * (1 - mageLeftClickIceSlowPercent), enemy.EntityRigidBody.velocity.y);
        }
        else if (enemy.EntityRigidBody.velocity.x != 0)
        {
            enemy.EntityRigidBody.velocity = new Vector2(0, enemy.EntityRigidBody.velocity.y);
        }
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
    }
}
