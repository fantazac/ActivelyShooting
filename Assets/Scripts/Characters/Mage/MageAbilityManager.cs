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

    protected override void Awake()
    {
        base.Awake();

        abilities = new Ability[] { gameObject.AddComponent<MageQ>(), gameObject.AddComponent<MageE>(), gameObject.AddComponent<MageLeftClick>(), gameObject.AddComponent<MageRightClick>() };
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
            abilities[2].ChangeType((int)MageMagic.Fire);
        }
        else if (selectedMagic == MageMagic.Fire)
        {
            selectedMagic = MageMagic.Ice;
            abilities[2].ChangeType((int)MageMagic.Ice);
        }
        else if (selectedMagic == MageMagic.Ice)
        {
            selectedMagic = MageMagic.Light;
            abilities[2].ChangeType((int)MageMagic.Light);
        }
        else
        {
            selectedMagic = MageMagic.Classic;
            abilities[2].ChangeType((int)MageMagic.Classic);
        }
    }
}

public enum MageMagic
{
    Classic,
    Fire,
    Ice,
    Light
}
