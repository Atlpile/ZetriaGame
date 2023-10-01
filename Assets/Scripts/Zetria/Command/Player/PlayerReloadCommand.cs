using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerReloadCommand : BaseCommand
    {
        public E_PlayerStatus status;

        public PlayerReloadCommand(E_PlayerStatus status)
        {
            this.status = status;
        }

        protected override void OnExecute()
        {
            var model = GameStructure.GetModel<IAmmoModel>();
            var panel = Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>();
            var audioManager = Manager.GetManager<IAudioManager>();

            switch (status)
            {
                case E_PlayerStatus.NPC:
                case E_PlayerStatus.Pistol:
                    audioManager.AudioPlay(FrameCore.E_AudioType.Effect, "pistol_reload");
                    model.ReloadPistolAmmo();
                    panel?.UpdatePistolAmmoText(model.PistlAmmoInfo.currentCount, model.PistlAmmoInfo.maxCount);
                    break;
                case E_PlayerStatus.ShortGun:
                    audioManager.AudioPlay(FrameCore.E_AudioType.Effect, "shotgun_reload");
                    model.ReloadShotGunAmmo();
                    panel?.UpdateShortGunAmmoText(model.ShortGunAmmoInfo.currentCount, model.ShortGunAmmoInfo.maxCount);
                    break;
            }
        }
    }

}
