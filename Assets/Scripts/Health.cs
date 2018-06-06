using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;
    private float maxHealth;

    public void Reduce(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        if (StaticObjects.Player.PhotonView.isMine)
        {
            SendToServer_CurrentHealth(-amount);
        }
        if (IsDead() && !gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }

    public void Restore(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (StaticObjects.Player.PhotonView.isMine)
        {
            SendToServer_CurrentHealth(amount);
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    protected void SendToServer_CurrentHealth(float amount)
    {
        StaticObjects.Player.PhotonView.RPC("ReceiveFromServer_CurrentHealth", PhotonTargets.Others, amount);
    }

    [PunRPC]
    private void ReceiveFromServer_CurrentHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (IsDead() && !gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }
}
