using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHitboxManager : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitbox;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();
            if (player.PhotonView.isMine && !player.IsInGracePeriod)
            {
                player.Health.Reduce(10);
            }
        }
    }
}
