using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField]
    private TriggerGroupManager triggerGroup;
    [SerializeField]
    private float wallXMovementOnTrigger;
    [SerializeField]
    private float wallYMovementOnTrigger;

    private float wallMovementSpeed;
    private Vector3 targetPosition;

    private WallManager()
    {
        wallMovementSpeed = 4;
    }

    private void Awake()
    {
        triggerGroup.OnPressedTriggers += OnPressedTriggers;

        targetPosition = new Vector2(transform.position.x + wallXMovementOnTrigger, transform.position.y + wallYMovementOnTrigger);
    }

    private void OnPressedTriggers()
    {
        StartCoroutine(MoveWall());
    }

    private IEnumerator MoveWall()
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wallMovementSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
