using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public static class UIManagerExtensions
    {
        public static void AddPanels_FromFolder(IUIManager manager)
        {
            GameObject[] assets = manager.ResourcesManager.LoadAssetsFolder<GameObject>(E_ResourcesPath.UI, false);
            foreach (var item in assets)
            {
                manager.ObjectPoolManager.AddObject(item);
            }
        }
    }
}


