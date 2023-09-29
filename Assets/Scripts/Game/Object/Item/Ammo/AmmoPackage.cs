using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class AmmoPackage : BaseComponent
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;
        public E_AmmoType ammoType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Player")
            {
                GameStructure.SendCommand(new PickUpAmmoPackageCommand(ammoType));
                Destroy(this.gameObject);
            }
        }
    }
}


