using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbilityManager : MonoBehaviour
{
    protected Player player;

    protected bool leftClickIsPressed;

    protected void Awake()
    {
        player = GetComponent<Player>();

        if (player.PlayerInputManager)
        {
            player.PlayerInputManager.OnQPressed += UseQAbility;
            player.PlayerInputManager.OnEPressed += UseEAbility;
            player.PlayerInputManager.OnLeftClick += UseBasicAttack;
            player.PlayerInputManager.OnRightClick += UseSpecialAttack;
            player.PlayerInputManager.OnTabPressed += SwitchWeapon;
        }
    }

    protected abstract void UseQAbility();                                      //Q
    protected abstract void UseEAbility();                                      //E
    protected abstract void UseBasicAttack(Vector3 position, bool isPressed);   //LeftClick
    protected abstract void UseSpecialAttack(Vector3 position);                 //RightClick
    protected abstract void SwitchWeapon();                                     //Tab?
}
