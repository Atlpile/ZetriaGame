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

        private ICommand[] commands;

        public override void OnInit()
        {
            commands = new ICommand[]
            {
                new NewGameCommand(),
                new ContinueGameCommand(),
                new OpenSettingPanelCommand()
            };
        }

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
                    GameStructure.SendCommand(commands[0]);
                    break;
                case nameof(btnContinue):
                    GameStructure.SendCommand(commands[1]);
                    break;
                case nameof(btnOptions):
                    GameStructure.SendCommand(commands[2]);
                    break;
                case nameof(btnExit):

                    break;
            }
        }
    }
}

