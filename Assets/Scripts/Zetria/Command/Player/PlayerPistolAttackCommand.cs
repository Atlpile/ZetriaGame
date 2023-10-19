using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerPistolAttackCommand : BaseCommand
    {
        private bool isCrouch;
        private bool isRight;
        private Transform playerTransform;

        public PlayerPistolAttackCommand(bool isRight, bool isCrouch, Transform playerTransform)
        {
            this.isCrouch = isCrouch;
            this.isRight = isRight;
            this.playerTransform = playerTransform;
        }

        protected override void OnExecute()
        {
            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "pistol_fire");

            var model = GameStructure.GetModel<IAmmoModel>();
            model.PistlAmmoInfo.currentCount--;

            var panel = Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>();
            panel?.UpdatePistolAmmoText(model.PistlAmmoInfo.currentCount, model.PistlAmmoInfo.maxCount);

            var bullet = Manager.GetManager<IObjectPoolManager>().GetObject("PistolBullet").GetComponent<PlayerBullet>();
            bullet.SetBulletTransform(isRight, isCrouch, playerTransform);
        }
    }
}



