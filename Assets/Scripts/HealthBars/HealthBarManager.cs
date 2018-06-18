using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;

    private List<Entity> entities;
    private List<HealthBar> healthBars;

    private HealthBarManager()
    {
        entities = new List<Entity>();
        healthBars = new List<HealthBar>();
    }

    private void Start()
    {
        foreach (Entity entity in FindObjectsOfType<Entity>())
        {
            entity.HealthBar = SetupHealthBarForEntity(entity);
        }
    }

    public HealthBar SetupHealthBarForEntity(Entity entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);

            HealthBar healthBar = Instantiate(healthBarPrefab).GetComponent<HealthBar>();
            healthBar.SetupHealthBar(entity);
            healthBar.transform.SetParent(transform, false);
            healthBars.Add(healthBar);
            return healthBar;
        }
        return null;
    }

    public void RemoveHealthBarOfDeletedEntity(Entity entity)
    {
        foreach (HealthBar healthBar in healthBars)
        {
            if (healthBar.GetEntity() == entity)
            {
                healthBars.Remove(healthBar);
                entities.Remove(entity);
                Destroy(healthBar.gameObject);
                break;
            }
        }
    }
}
