using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAbilityManager : PlayerAbilityManager
{
    private MageMagic selectedMagic;

    private MageAbilityManager()
    {
        selectedMagic = MageMagic.Classic;
    }

    private void OnGUI()
    {
        if (player.PhotonView.isMine)
        {
            GUILayout.Label("");//Ping
            GUILayout.Label("");//Position
            GUILayout.Label(selectedMagic + "");
        }
    }

    protected override void SwitchWeapon()
    {
        if (selectedMagic == MageMagic.Classic)
        {
            selectedMagic = MageMagic.Fire;
        }
        else if (selectedMagic == MageMagic.Fire)
        {
            selectedMagic = MageMagic.Light;
        }
        else if (selectedMagic == MageMagic.Light)
        {
            selectedMagic = MageMagic.Ice;
        }
        else
        {
            selectedMagic = MageMagic.Classic;
        }
    }
}

public enum MageMagic
{
    Classic,
    Fire,
    Light,
    Ice
}
