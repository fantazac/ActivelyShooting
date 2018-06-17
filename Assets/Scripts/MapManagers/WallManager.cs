using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField]
    private NextLevelTriggerGroupManager triggerGroup;
    [SerializeField]
    private float wallXMovementOnTrigger;
    [SerializeField]
    private float wallYMovementOnTrigger;

    private float wallMovementSpeed;
    private Vector3 targetPosition;

    private WallManager()
    {
        wallMovementSpeed = 6;
    }

    private void Awake()
    {
        targetPosition = new Vector2(transform.position.x + wallXMovementOnTrigger, transform.position.y + wallYMovementOnTrigger);
    }

    public void OpenWall()
    {
        StartCoroutine(MoveWall());
    }

    public void StopWallOpening()
    {
        if (transform.position != targetPosition)
        {
            StopAllCoroutines();
            transform.position = targetPosition;
        }
    }

    private IEnumerator MoveWall()
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, wallMovementSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
