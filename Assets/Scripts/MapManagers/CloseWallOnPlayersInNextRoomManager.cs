using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWallOnPlayersInNextRoomManager : MonoBehaviour
{
    [SerializeField]
    private WallManager wallManager;
    [SerializeField]
    private CloseWallManager closeWallManager;

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            bool readyToCloseWall = true;
            foreach (Player p in StaticObjects.Player.Party)
            {
                if (p.transform.position.x < transform.position.x)
                {
                    readyToCloseWall = false;
                    break;
                }
            }
            if (readyToCloseWall)
            {
                wallManager.StopWallOpening();
                closeWallManager.CloseWall();
                Destroy(gameObject);
            }
        }
    }
}
