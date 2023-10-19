using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerShortGunAttackCommand : BaseCommand
    {
        private bool isCrouch;
        private bool isRight;
        private Transform playerTransform;

        public PlayerShortGunAttackCommand(bool isRight, bool isCrouch, Transform playerTransform)
        {
            this.isCrouch = isCrouch;
            this.isRight = isRight;
            this.playerTransform = playerTransform;
        }

        protected override void OnExecute()
        {
            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "shotgun_fire");
            GameStructure.GetModel<IAmmoModel>().ShortGunAmmoInfo.currentCount--;

            GameObject[] bulletObjs = Manager.GetManager<IObjectPoolManager>().GetObjects("ShortGunBullet", 3);
            foreach (var obj in bulletObjs)
            {
                obj.GetComponent<PlayerBullet>().SetBulletTransform(isRight, isCrouch, playerTransform);
            }
        }
    }
}



