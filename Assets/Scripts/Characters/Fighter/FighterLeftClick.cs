﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterLeftClick : Ability
{
    private float speed;
    private float range;

    private bool canAttackInCycle;
    private bool readyToAttack;
    private FighterMode nextAttackMode;

    private bool isPressed;

    private float swordsmanCooldown;
    private float swordsmanDamage;

    private float tankCooldown;
    private float tankDamage;

    private float horizontalSpeedPercentOnLeftClickActive;

    private bool fighterQIsActive;

    private Vector3 lastMousePosition;
    private bool attackRightSide;

    private FighterMode selectedMode;

    private FighterLeftClick()
    {
        swordsmanCooldown = 0.4f;
        swordsmanDamage = 25;

        tankCooldown = 0.8f;
        tankDamage = 40;

        horizontalSpeedPercentOnLeftClickActive = 0.5f;

        ChangeType((int)FighterMode.Swordsman);

        IsHoldDownAbility = true;
    }

    protected override void UseAbilityEffect(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        if (this.isPressed != isPressed && !fighterQIsActive)
        {
            player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed ? horizontalSpeedPercentOnLeftClickActive : 1);
        }
        this.isPressed = isPressed;
        lastMousePosition = mousePosition;
    }

    public override void UseAbility(Vector3 mousePosition, bool isPressed, bool forceAbility = false)
    {
        UseAbilityEffect(mousePosition, isPressed);
    }

    private IEnumerator AttackOnNetwork()
    {
        cooldownRemaining = cooldown;
        attackRightSide = lastMousePosition.x >= transform.position.x;

        yield return null;

        while (cooldownRemaining > cooldown * 0.5f)
        {
            cooldownRemaining -= Time.deltaTime;

            yield return null;
        }

        Attack();
    }

    public void SetFighterQIsActive(bool active)
    {
        fighterQIsActive = active;
        if (active)
        {
            cooldownRemaining = 0;
        }
        cooldown = (active || selectedMode == FighterMode.Swordsman ? swordsmanCooldown : tankCooldown) * cooldownReduction;
        player.PlayerMovementManager.ChangeHorizontalSpeed(isPressed && !active ? horizontalSpeedPercentOnLeftClickActive : 1);
    }

    public override void ChangeType(int mode)
    {
        if (mode == (int)FighterMode.Swordsman)
        {
            selectedMode = FighterMode.Swordsman;
            cooldown = swordsmanCooldown * cooldownReduction;
        }
        else
        {
            selectedMode = FighterMode.Tank;
            cooldown = (fighterQIsActive ? swordsmanCooldown : tankCooldown) * cooldownReduction;
        }
    }

    public override void SetCooldownReduction(float cooldownReduction)
    {
        this.cooldownReduction = cooldownReduction;
        if (selectedMode == FighterMode.Swordsman)
        {
            cooldown = swordsmanCooldown * cooldownReduction;
        }
        else
        {
            cooldown = tankCooldown * cooldownReduction;
        }
        if (cooldownRemaining > cooldown)
        {
            cooldownRemaining = cooldown;
        }
    }

    private void Update()
    {
        if (player.PhotonView.isMine)
        {
            if (isPressed && !IsOnCooldown)
            {
                StartCooldown();
                nextAttackMode = selectedMode;
                if (fighterQIsActive)
                {
                    Attack();
                }
            }
            else if (IsOnCooldown && readyToAttack)
            {
                readyToAttack = false;
                Attack();
            }
        }
    }

    protected override IEnumerator PutAbilityOffCooldown()
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldown;
        canAttackInCycle = true;
        attackRightSide = lastMousePosition.x >= transform.position.x;

        yield return null;

        //player.AbilityUIManager.SetAbilityOnCooldown(AbilityCategory, ID, IsBlocked);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            if (!fighterQIsActive && canAttackInCycle && cooldownRemaining <= cooldown * 0.5f)
            {
                canAttackInCycle = false;
                readyToAttack = true;
            }

            //player.AbilityUIManager.UpdateAbilityCooldown(AbilityCategory, ID, cooldownOnStart, cooldownRemaining);

            yield return null;
        }

        //player.AbilityUIManager.SetAbilityOffCooldown(AbilityCategory, ID, UsesResource, IsEnabled, IsBlocked);
        IsOnCooldown = false;
    }

    private void Attack()
    {
        if (!fighterQIsActive)
        {
            float damage = nextAttackMode == FighterMode.Swordsman ? swordsmanDamage : tankDamage;
            foreach (Collider2D collider in Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackRightSide ? 0.65f : -0.65f), transform.position.y + 0.35f),
            new Vector2(1.65f, 1.3f), 0, LayerMask.GetMask("Enemies")))
            {
                collider.GetComponent<Health>().Reduce(damage * damageAmplification);
            }
        }
        else
        {
            player.PlayerAbilityManager.UseAbilityWithoutLimitation(3, Input.mousePosition);
        }
    }
}
