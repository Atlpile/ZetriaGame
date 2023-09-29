using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerShortGunAttackCommand : BaseCommand
    {
        private Action<GameObject> OnSetBulletPosition;

        public PlayerShortGunAttackCommand(Action<GameObject> OnSetBulletPosition)
        {
            this.OnSetBulletPosition = OnSetBulletPosition;
        }

        protected override void OnExecute()
        {

        }
    }
}



