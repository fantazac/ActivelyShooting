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
        structureSpeed = 25;
    }

    private void Awake()
    {
        initialPosition = transform.position;
        targetPosition = new Vector2(transform.position.x + structureXMovementOnTrigger, transform.position.y + structureYMovementOnTrigger);
    }

    public void SwitchInitialAndTargetPositions()
    {
        Vector3 tempPosition = initialPosition;
        initialPosition = targetPosition;
        targetPosition = tempPosition;
    }

    public void MoveStructureToTargetPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveStructure(targetPosition, structureSpeed));
    }

    public void MoveStructureBackToInitialPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveStructure(initialPosition, structureSpeed));
    }

    private IEnumerator MoveStructure(Vector3 position, float speed)
    {
        while (transform.position != position)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime);

            yield return null;
        }
    }
}
