using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FrameCore;

namespace Zetria
{
    public class FrameworkGameEntry : BaseComponent
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private void Start()
        {
            GameStructure.SendCommand(new StartGameCommand());

            // GameObject managerObj = Resources.Load<GameObject>("Misc/MonoManager");
            // IMonoManager manager = managerObj.GetComponent<IMonoManager>();
            // manager.OnInitManagerAction = StartGame;
            // Instantiate(managerObj);
        }

        private void StartGame()
        {

        }
    }
}


