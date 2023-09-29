using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using UnityEngine.UI;

namespace Zetria
{
    public class PauseUIPanel : BaseUIPanel
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private Button btnResume;
        private Button btnRestartLevel;
        private Button btnOptions;
        private Button btnTitleScreen;
        private Button btnBugReport;
        private Button btnExit;

        protected override void OnGetUIComponent()
        {
            GetChildAllUIComponent<Button>();

            btnResume = GetUIComponent<Button>(nameof(btnResume));
            btnRestartLevel = GetUIComponent<Button>(nameof(btnRestartLevel));
            btnOptions = GetUIComponent<Button>(nameof(btnOptions));
            btnTitleScreen = GetUIComponent<Button>(nameof(btnTitleScreen));
            btnBugReport = GetUIComponent<Button>(nameof(btnBugReport));
            btnExit = GetUIComponent<Button>(nameof(btnExit));
        }

        protected override void OnClick(string buttonName)
        {
            switch (buttonName)
            {
                case nameof(btnResume):
                    break;
                case nameof(btnRestartLevel):
                    break;
                case nameof(btnOptions):
                    break;
                case nameof(btnTitleScreen):
                    break;
                case nameof(btnBugReport):
                    break;
                case nameof(btnExit):
                    break;
            }
        }
    }
}


