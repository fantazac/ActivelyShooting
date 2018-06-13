using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;

    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Reduce(float amount)
    {
        ReduceHealth(amount);
        entity.ChangeHealthOnServer(true, amount);
    }

    public void ReduceFromServer(float amount)
    {
        ReduceHealth(amount);
    }

    private void ReduceHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        if (IsDead() && entity is Enemy)
        {
            if (entity.PhotonView.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                entity.gameObject.SetActive(false);
            }
        }
    }

    public void Restore(float amount, bool isPercent = false)
    {
        RestoreHealth(amount, isPercent);
        entity.ChangeHealthOnServer(false, amount, isPercent);
    }

    public void RestoreFromServer(float amount, bool isPercent = false)
    {
        RestoreHealth(amount, isPercent);
    }

    private void RestoreHealth(float amount, bool isPercent)
    {
        currentHealth = Mathf.Clamp(currentHealth + (isPercent ? currentHealth * amount : amount), 0, maxHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
