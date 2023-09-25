using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public interface IAmmoSystem : ISystem
    {

    }

    public class AmmoSystem : BaseSystem, IAmmoSystem
    {
        private AmmoInfo _ammoInfo;

        protected override void OnInit()
        {
            _ammoInfo = new AmmoInfo();
        }

        public void PickUpPistolAmmoPackage()
        {

        }

        public void PickUpShotGunAmmoPackage()
        {

        }

    }

}



