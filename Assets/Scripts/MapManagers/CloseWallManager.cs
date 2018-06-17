using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWallManager : MonoBehaviour
{
    [SerializeField]
    private float wallXMovementOnTrigger;
    [SerializeField]
    private float wallYMovementOnTrigger;

    private GameObject sprite;

    private float wallMovementSpeed;
    private Vector3 targetPosition;

    private CloseWallManager()
    {
        wallMovementSpeed = 7;
    }

    private void Awake()
    {
        sprite = transform.GetChild(0).gameObject;

        targetPosition = transform.position;
    }

    public void CloseWall()
    {
        transform.position = targetPosition;
        sprite.transform.position -= Vector3.up * wallYMovementOnTrigger;
        StartCoroutine(MoveWall());
    }

    private IEnumerator MoveWall()
    {
        while (sprite.transform.position != targetPosition)
        {
            sprite.transform.position = Vector2.MoveTowards(sprite.transform.position, targetPosition, wallMovementSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
