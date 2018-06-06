using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAbilityManager : PlayerAbilityManager
{
    private GunnerWeapon selectedWeapon;

    private GunnerAbilityManager()
    {
        selectedWeapon = GunnerWeapon.Minigun;
    }

    protected override void Awake()
    {
        base.Awake();

        abilities = new Ability[] { null, null, gameObject.AddComponent<GunnerLeftClick>(), gameObject.AddComponent<GunnerRightClick>() };
    }

    private void OnGUI()
    {
        if (player.PhotonView.isMine)
        {
            GUILayout.Label("");//Ping
            GUILayout.Label("");//Position
            GUILayout.Label(selectedWeapon + "");
        }
    }

    protected override void SwitchWeapon()
    {
        if (selectedWeapon == GunnerWeapon.Minigun)
        {
            selectedWeapon = GunnerWeapon.RocketLauncher;
            abilities[2].ChangeWeapon((int)GunnerWeapon.RocketLauncher);
        }
        else
        {
            selectedWeapon = GunnerWeapon.Minigun;
            abilities[2].ChangeWeapon((int)GunnerWeapon.Minigun);
        }
    }
}

public enum GunnerWeapon
{
    Minigun,
    RocketLauncher
}
