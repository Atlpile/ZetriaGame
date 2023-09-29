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
            Debug.Log("开始新游戏");
            var UIManager = Manager.GetManager<IUIManager>();

            UIManager.HidePanel<MainUIPanel>();
            Manager.GetManager<ISceneLoader>().LoadSceneAsync("FrameWorkTestScene", () =>
            {
                UIManager.ShowPanel<GameUIPanel>();
            });

            //清除场景信息
            //加载LoadingPanel
        }
    }
}



