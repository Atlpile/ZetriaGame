using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PickUpAmmoPackageCommand : BaseCommand
    {
        public E_AmmoType ammoType;

        public PickUpAmmoPackageCommand(E_AmmoType type)
        {
            this.ammoType = type;
        }

        protected override void OnExecute()
        {
            var model = GameStructure.GetModel<IAmmoModel>();
            var panel = Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>();

            switch (ammoType)
            {
                case E_AmmoType.Pistol:
                    model.PistlAmmoInfo.maxCount += model.PistlAmmoInfo.packageCount;
                    panel?.UpdatePistolAmmoText(
                          model.PistlAmmoInfo.currentCount,
                          model.PistlAmmoInfo.maxCount
                      );
                    break;
                case E_AmmoType.ShotGun:
                    model.ShortGunAmmoInfo.maxCount += model.ShortGunAmmoInfo.packageCount;
                    panel?.UpdatePistolAmmoText(
                        model.ShortGunAmmoInfo.currentCount,
                        model.ShortGunAmmoInfo.maxCount
                    );
                    break;
            }

            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "pistol_ammo_collect");
        }
    }
}

