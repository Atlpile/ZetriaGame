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
            var UIManager = Manager.GetManager<IUIManager>();
            UIManager.AddPanel<MainUIPanel>();
            UIManager.ShowPanel<MainUIPanel>();

        }
    }
}



