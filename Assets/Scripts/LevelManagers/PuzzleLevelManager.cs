using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLevelManager : MonoBehaviour
{
    [SerializeField]
    private TriggerGroupManager nextLevelTriggerGroupManager;
    [SerializeField]
    private WallManager nextLevelWallManager;

    [SerializeField]
    private List<TriggerGroupManager> triggerGroupManagers;
    [SerializeField]
    private List<PuzzleStructureManager> puzzleStructureManagers;

    private void Awake()
    {
        nextLevelTriggerGroupManager.OnTriggerGroupPressed += OnNextLevelTriggerGroupPressed;

        for (int i = 0; i < triggerGroupManagers.Count; i++)
        {
            TriggerGroupManager tgm = triggerGroupManagers[i];
            tgm.ID = i;
            tgm.OnTriggerGroupPressed += OnTriggerGroupPressed;
            if (tgm.GetDisableOnPressed() && i % 2 == 1)
            {
                tgm.gameObject.SetActive(false);
            }
        }
    }

    private void OnNextLevelTriggerGroupPressed(bool activated, int id)
    {
        nextLevelTriggerGroupManager.gameObject.SetActive(false);
        nextLevelWallManager.OpenWall();
    }

    private void OnTriggerGroupPressed(bool activated, int id)
    {
        if (activated)
        {
            puzzleStructureManagers[id].MoveStructureToTargetPosition();
            if (triggerGroupManagers[id].GetDisableOnPressed())
            {
                int nextId = id % 2 == 0 ? 1 : -1;
                triggerGroupManagers[id].gameObject.SetActive(false);
                triggerGroupManagers[id + nextId].gameObject.SetActive(true);
                puzzleStructureManagers[id].SwitchInitialAndTargetPositions();
            }
        }
        else if (triggerGroupManagers[id].gameObject.activeSelf)
        {
            puzzleStructureManagers[id].MoveStructureBackToInitialPosition();
        }
    }
}
