using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;


namespace Zetria
{
    public class PlayerMeleeAttackCommand : BaseCommand
    {
        private Action<GameObject> OnSetFXPosition;

        public PlayerMeleeAttackCommand(Action<GameObject> OnSetFXPosition)
        {
            this.OnSetFXPosition = OnSetFXPosition;
        }

        protected override void OnExecute()
        {
            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "player_meleeAttack");
            GameObject kickFX = Manager.GetManager<IObjectPoolManager>().GetObject("FX_Kick");
            OnSetFXPosition?.Invoke(kickFX);
        }
    }
}



