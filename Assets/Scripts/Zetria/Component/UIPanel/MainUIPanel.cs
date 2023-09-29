using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using UnityEngine.UI;

namespace Zetria
{
    public class MainUIPanel : BaseUIPanel
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private Button btnNewGame;
        private Button btnContinue;
        private Button btnOptions;
        private Button btnExit;

        protected override void OnGetUIComponent()
        {
            GetChildAllUIComponent<Button>();

            btnNewGame = GetUIComponent<Button>(nameof(btnNewGame));
            btnContinue = GetUIComponent<Button>(nameof(btnContinue));
            btnOptions = GetUIComponent<Button>(nameof(btnOptions));
            btnExit = GetUIComponent<Button>(nameof(btnExit));
        }

        protected override void OnClick(string buttonName)
        {
            switch (buttonName)
            {
                case nameof(btnNewGame):
                    GameStructure.SendCommand(new NewGameCommand());
                    break;
                case nameof(btnContinue):
                    GameStructure.SendCommand(new ContinueGameCommand());
                    break;
                case nameof(btnOptions):

                    break;
                case nameof(btnExit):

                    break;
            }
        }
    }
}

