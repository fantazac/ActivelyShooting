﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    private Entity entity;

    private Vector2 healthBarOffset;
    private Vector2 characterYOffset;

    private Camera playerCamera;

    private float maxHealth;

    private void Start()
    {
        playerCamera = StaticObjects.PlayerCamera;
        healthBarOffset = Vector2.right * Screen.width * -0.5f + Vector2.up * Screen.height * -0.5f;
        characterYOffset = Vector2.up * 40;
    }

    public void SetupHealthBar(Entity entity)
    {
        this.entity = entity;

        maxHealth = entity.Health.GetMaxHealth();
        entity.Health.OnHealthChanged += OnCurrentHealthChanged;

        if (entity is Player)
        {
            if (StaticObjects.Player == entity)
            {
                healthImage.color = Color.green;
            }
            else
            {
                healthImage.color = Color.blue;
            }
        }
        else
        {
            healthImage.color = Color.red;
        }
    }

    private void OnDestroy()
    {
        entity.Health.OnHealthChanged -= OnCurrentHealthChanged;
    }

    public Entity GetEntity()
    {
        return entity;
    }

    private void LateUpdate()
    {
        if (entity)
        {
            Vector3 position = playerCamera.WorldToScreenPoint(entity.transform.position);
            GetComponent<RectTransform>().anchoredPosition =
                Vector2.right * (position.x + healthBarOffset.x) +
                Vector2.up * (position.y + healthBarOffset.y) +
                characterYOffset;
        }
        else
        {
            enabled = false;
        }
    }

    private void OnCurrentHealthChanged()
    {
        healthImage.fillAmount = entity.Health.GetCurrentHealth() / maxHealth;
    }
}
