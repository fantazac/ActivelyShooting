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

    public delegate void OnHealthChangedHandler(int id, bool reduce, float amount, bool isPercent = false);
    public event OnHealthChangedHandler OnHealthChanged;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Reduce(float amount)
    {
        ReduceHealth(amount);
        if (OnHealthChanged != null)
        {
            OnHealthChanged(entity.ID, true, amount);
        }
        if (entity.PhotonView)
        {
            entity.ChangeHealthOnServer(true, amount);
        }
    }

    public void ReduceFromServer(float amount)
    {
        ReduceHealth(amount);
    }

    private void ReduceHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        entity.HealthBar.UpdateHealthBar();
    }

    public void Restore(float amount, bool isPercent = false)
    {
        RestoreHealth(amount, isPercent);
        if (OnHealthChanged != null)
        {
            OnHealthChanged(entity.ID, false, amount, isPercent);
        }
        if (entity.PhotonView)
        {
            entity.ChangeHealthOnServer(false, amount, isPercent);
        }
    }

    public void RestoreFromServer(float amount, bool isPercent = false)
    {
        RestoreHealth(amount, isPercent);
    }

    private void RestoreHealth(float amount, bool isPercent)
    {
        currentHealth = Mathf.Clamp(currentHealth + (isPercent ? currentHealth * amount : amount), 0, maxHealth);
        entity.HealthBar.UpdateHealthBar();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void SetCurrentHealthOnLoad(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }
}
