using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerDamageCommand : BaseCommand
    {
        public int currentHealth;
        public int maxHealth;

        public PlayerDamageCommand(int currentHealth, int maxHealth)
        {
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;
        }

        protected override void OnExecute()
        {
            Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "player_hurt_1");
            Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>().UpdateLifeBar(currentHealth, maxHealth);
            Manager.GetManager<IInputManager>().CanInput = false;
        }
    }
}



