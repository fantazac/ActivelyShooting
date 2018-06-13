using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Reduce(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        SendToServer_CurrentHealth(-amount);
        if (IsDead() && !gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }

    public void Restore(float amount, bool isPercent = false)
    {
        currentHealth = Mathf.Clamp(currentHealth + (isPercent ? currentHealth * amount : amount), 0, maxHealth);
        SendToServer_CurrentHealth(amount, isPercent);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void SendToServer_CurrentHealth(float amount, bool isPercent = false)
    {
        StaticObjects.Player.PhotonView.RPC("ReceiveFromServer_CurrentHealth", PhotonTargets.Others, amount, isPercent);
    }

    [PunRPC]
    private void ReceiveFromServer_CurrentHealth(float amount, bool isPercent)//not working online
    {
        currentHealth = Mathf.Clamp(currentHealth + (isPercent ? currentHealth * amount : amount), 0, maxHealth);
        if (IsDead() && !gameObject.GetComponent<Player>())
        {
            Destroy(gameObject);
        }
    }
}
