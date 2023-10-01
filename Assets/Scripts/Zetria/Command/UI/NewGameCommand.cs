using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class NewGameCommand : BaseCommand
    {
        protected override void OnExecute()
        {
            var uiManager = Manager.GetManager<IUIManager>();
            var sceneLoader = Manager.GetManager<ISceneLoader>();
            var audioManager = Manager.GetManager<IAudioManager>();

            uiManager.HidePanel<MainUIPanel>();
            sceneLoader.LoadSceneAsync("FrameWorkTestScene", () =>
            {
                uiManager.ShowPanel<GameUIPanel>();
                audioManager.AudioPlay(FrameCore.E_AudioType.BGM, "bgm_02", true);
            });

            //清除场景信息
            //加载LoadingPanel
        }
    }
}



