using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IUIManager : IManager
    {

        IResourcesManager ResourcesManager { get; }
        IObjectPoolManager ObjectPoolManager { get; }

        void AddPanel<T>() where T : IPanel;
        void RemovePanel<T>() where T : IPanel;
        bool IsRegisteredPanel<T>() where T : IPanel;

        T ShowPanel<T>() where T : class, IPanel;
        void HidePanel<T>() where T : IPanel;
        T GetExistPanel<T>() where T : class, IPanel;

        T PushPanel<T>() where T : class, IPanel;
        bool PopPanel();
        IPanel PeekPanel();

    }
}

