using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class UIPanelTest : BaseComponent
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private void Start()
        {
            Manager.GetManager<IObjectPoolManager>().AddObject_FromResources(FrameCore.E_ResourcesPath.UI, "GameUIPanel");
            // Manager.GetManager<IObjectPoolManager>().AddObject_FromResources(FrameCore.E_ResourcesPath.UI, "MainUIPanel");
            // Manager.GetManager<IObjectPoolManager>().AddObject_FromResources(FrameCore.E_ResourcesPath.UI, "SettingUIPanel");
            // Manager.GetManager<IObjectPoolManager>().AddObject_FromResources(FrameCore.E_ResourcesPath.UI, "WarnQuitUIPanel");
            // Manager.GetManager<IObjectPoolManager>().AddObject_FromResources(FrameCore.E_ResourcesPath.UI, "WarnRestartUIPanel");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Manager.GetManager<IUIManager>().ShowPanel<GameUIPanel>();
                // Manager.GetManager<IUIManager>().ShowPanel<MainUIPanel>();
                // Manager.GetManager<IUIManager>().ShowPanel<SettingUIPanel>();
                // Manager.GetManager<IUIManager>().ShowPanel<WarnQuitUIPanel>();
                // Manager.GetManager<IUIManager>().ShowPanel<WarnRestartUIPanel>();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Manager.GetManager<IUIManager>().HidePanel<GameUIPanel>();
                // Manager.GetManager<IUIManager>().HidePanel<MainUIPanel>();
                // Manager.GetManager<IUIManager>().HidePanel<SettingUIPanel>();
                // Manager.GetManager<IUIManager>().HidePanel<WarnQuitUIPanel>();
                // Manager.GetManager<IUIManager>().HidePanel<WarnRestartUIPanel>();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {

            }
        }

        private void OnDeadEvent(PlayerDeadEvent e)
        {
            Debug.Log("触发Player死亡事件");
            Debug.Log("Int信息" + e.testEventInt);
            Debug.Log("String信息" + e.testEventString);
        }
    }
}

