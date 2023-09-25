using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerJumpCommand : BaseCommand
    {
        private Action<GameObject> OnSetFXPosition;

        public PlayerJumpCommand(Action<GameObject> OnSetFXPosition)
        {
            this.OnSetFXPosition = OnSetFXPosition;
        }

        protected override void OnExecute()
        {
            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "player_jump");
            GameObject jumpFX = Manager.GetManager<IObjectPoolManager>().GetObject("FX_Jump");
            OnSetFXPosition?.Invoke(jumpFX);
        }
    }
}



