using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class OpenSettingPanelCommand : BaseCommand
    {
        protected override void OnExecute()
        {
            Manager.GetManager<IUIManager>().ShowPanel<SettingUIPanel>();
        }
    }

}


