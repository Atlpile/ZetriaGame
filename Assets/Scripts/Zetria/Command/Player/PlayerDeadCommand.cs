using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerDeadCommand : BaseCommand
    {
        public int currentHealth;
        public int maxHealth;

        public PlayerDeadCommand(int currentHealth, int maxHealth)
        {
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;
        }

        protected override void OnExecute()
        {
            GameStructure.TriggerGameEvent<PlayerDeadEvent>(new PlayerDeadEvent());
            // GameManager.Instance.EventManager.EventTrigger(E_EventType.PlayerDead);
            Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>().UpdateLifeBar(currentHealth, maxHealth);
            // GameManager.Instance.SceneLoader.LoadCurrentSceneInGame();

        }
    }
}



