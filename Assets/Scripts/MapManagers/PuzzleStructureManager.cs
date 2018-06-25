using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStructureManager : MonoBehaviour
{
    [SerializeField]
    private float structureXMovementOnTrigger;
    [SerializeField]
    private float structureYMovementOnTrigger;

    private float structureSpeed;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private PuzzleStructureManager()
    {
        structureSpeed = 10;
    }

    private void Awake()
    {
        initialPosition = transform.position;
        targetPosition = new Vector2(transform.position.x + structureXMovementOnTrigger, transform.position.y + structureYMovementOnTrigger);
    }

    public void MoveStructureToTargetPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveStructure(targetPosition));
    }

    public void MoveStructureBackToInitialPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveStructure(initialPosition));
    }

    private IEnumerator MoveStructure(Vector3 position)
    {
        while (transform.position != position)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, structureSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
