using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

public class PlayerPistolFireCommand : BaseCommand
{
    private Action<GameObject> OnSetBulletPosition;

    public PlayerPistolFireCommand(Action<GameObject> OnSetBulletPosition)
    {
        this.OnSetBulletPosition = OnSetBulletPosition;
    }

    protected override void OnExecute()
    {
        Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "pistol_fire");
        GameObject pistolBullet = Manager.GetManager<IObjectPoolManager>().GetObject("PistolBullet");
    }
}
