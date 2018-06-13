using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    private bool affectedByMageLeftClickIceSlow;
    private float mageLeftClickIceSlowDurationRemaining;
    private float mageLeftClickIceSlowPercent;

    private float movementSpeed;

    private EnemyMovementManager()
    {
        movementSpeed = 1;
    }

    private void Update()
    {
        Debug.Log(movementSpeed * (1 - mageLeftClickIceSlowPercent));
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
}
