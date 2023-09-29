using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using UnityEngine.UI;

namespace Zetria
{
    public class WarnRestartUIPanel : BaseUIPanel
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private Button button_Yes;
        private Button button_No;
        protected override void OnGetUIComponent()
        {
            GetChildAllUIComponent<Button>();

            button_Yes = GetUIComponent<Button>(nameof(button_Yes));
            button_No = GetUIComponent<Button>(nameof(button_No));
        }

        protected override void OnClick(string buttonName)
        {
            switch (buttonName)
            {
                case nameof(button_Yes):
                    // RestartCurrentLevel();
                    break;
                case nameof(button_No):
                    // NotRestart();
                    break;
            }
        }
    }
}


