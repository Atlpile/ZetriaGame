using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class StartGameCommand : BaseCommand
    {
        protected override void OnExecute()
        {
            var uiManager = Manager.GetManager<IUIManager>();
            var audioManager = Manager.GetManager<IAudioManager>();

            uiManager.AddPanel<MainUIPanel>();
            uiManager.AddPanel<SettingUIPanel>();
            uiManager.AddPanel<GameUIPanel>();
            uiManager.AddPanel<WarnQuitUIPanel>();
            uiManager.AddPanel<WarnRestartUIPanel>();

            uiManager.ShowPanel<MainUIPanel>();
            audioManager.AudioPlay(FrameCore.E_AudioType.BGM, "bgm_01", true);
        }
    }
}



