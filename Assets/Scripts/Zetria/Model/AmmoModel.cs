using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public interface IAmmoModel : IModel
    {
        AmmoInfo PistlAmmoInfo { get; set; }
        AmmoInfo ShortGunAmmoInfo { get; set; }

        void ReloadPistolAmmo();
        void ReloadShotGunAmmo();
        bool CanReload(E_PlayerStatus status);
        bool CanFireAttack(E_PlayerStatus status);
    }

    public class AmmoModel : BaseModel, IAmmoModel
    {
        public AmmoInfo PistlAmmoInfo { get; set; }
        public AmmoInfo ShortGunAmmoInfo { get; set; }

        protected override void OnInit()
        {
            PistlAmmoInfo = new AmmoInfo(16, 8);
            ShortGunAmmoInfo = new AmmoInfo(8, 4);
        }

        public void ReloadPistolAmmo()
        {
            for (int i = PistlAmmoInfo.currentCount; i < PistlAmmoInfo.ammoLimit; i++)
            {
                if (PistlAmmoInfo.maxCount == 0) break;
                PistlAmmoInfo.maxCount--;
                PistlAmmoInfo.currentCount++;
            }
        }

        public void ReloadShotGunAmmo()
        {
            for (int i = ShortGunAmmoInfo.currentCount; i < ShortGunAmmoInfo.ammoLimit; i++)
            {
                if (ShortGunAmmoInfo.maxCount == 0) break;
                ShortGunAmmoInfo.maxCount--;
                ShortGunAmmoInfo.currentCount++;
            }
        }

        public bool CanReload(E_PlayerStatus status)
        {
            switch (status)
            {
                case E_PlayerStatus.Pistol:
                    if (PistlAmmoInfo.maxCount > 0 && PistlAmmoInfo.currentCount != PistlAmmoInfo.ammoLimit)
                        return true;
                    break;
                case E_PlayerStatus.ShortGun:
                    if (ShortGunAmmoInfo.maxCount > 0 && ShortGunAmmoInfo.currentCount != ShortGunAmmoInfo.ammoLimit)
                        return true;
                    break;
            }
            return false;
        }

        public bool CanFireAttack(E_PlayerStatus status)
        {
            switch (status)
            {
                case E_PlayerStatus.NPC:
                case E_PlayerStatus.Pistol:
                    if (PistlAmmoInfo.currentCount != 0)
                        return true;
                    break;
                case E_PlayerStatus.ShortGun:
                    if (ShortGunAmmoInfo.currentCount != 0)
                        return true;
                    break;
            }
            return false;
        }
    }
}




