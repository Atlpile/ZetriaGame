using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class UIManager : BaseManager, IUIManager
    {
        private readonly Dictionary<string, IPanel> _PanelContainer = new();
        private readonly Stack<IPanel> _PanelStack = new();
        private RectTransform _canvasRect;

        public IResourcesManager ResourcesManager => Manager.GetManager<IResourcesManager>();

        public IObjectPoolManager ObjectPoolManager => Manager.GetManager<IObjectPoolManager>();


        public UIManager(MonoManager manager) : base(manager)
        {

        }

        protected override void OnInit()
        {
            Transform canvas = Manager.transform.Find("Canvas");
            _canvasRect = canvas.transform as RectTransform;
        }

        public void AddPanel<T>() where T : IPanel
        {
            string panelName = typeof(T).Name;
            ObjectPoolManager.AddObject_FromResources(E_ResourcesPath.UI, panelName);
        }

        public void RemovePanel<T>() where T : IPanel
        {
            string panelName = typeof(T).Name;
            ObjectPoolManager.RemovePoolStack(panelName);
        }

        public bool IsRegisteredPanel<T>() where T : IPanel
        {
            string panelName = typeof(T).Name;
            return ObjectPoolManager.GetPoolStackExists(panelName);
        }

        public T ShowPanel<T>() where T : class, IPanel
        {
            string panelName = typeof(T).Name;
            if (_PanelContainer.ContainsKey(panelName))
            {
                Debug.LogWarning("UIManager: 场景中存在" + panelName + ", 无法再显示该面板");
                return _PanelContainer[panelName] as T;
            }
            else
            {
                GameObject panelObj = ObjectPoolManager.GetObject(panelName, _canvasRect);
                InitPanelRect(panelObj);
                IPanel panel = panelObj.GetComponent<IPanel>();
                panel.Show();
                _PanelContainer.Add(panelName, panel);
                return panel as T;
            }
        }

        public void HidePanel<T>() where T : IPanel
        {
            string panelName = typeof(T).Name;
            if (_PanelContainer.ContainsKey(panelName))
            {
                _PanelContainer[panelName].Hide();
                _PanelContainer.Remove(panelName);
            }
            else
            {
                Debug.LogWarning("UIManager: 场景中不存在" + panelName + ", 无法隐藏该面板");
            }
        }

        public T GetExistPanel<T>() where T : class, IPanel
        {
            string panelName = typeof(T).Name;
            if (_PanelContainer.ContainsKey(panelName))
            {
                return _PanelContainer[panelName] as T;
            }
            else
            {
                Debug.LogWarning("UIManager:场景中不存在" + panelName);
                return null;
            }
        }

        public T PushPanel<T>() where T : class, IPanel
        {
            string panelName = typeof(T).Name;
            GameObject panelObj = ObjectPoolManager.GetObject(panelName, _canvasRect);
            InitPanelRect(panelObj);
            IPanel panel = panelObj.GetComponent<IPanel>();
            panel.Show();
            _PanelStack.Push(panel);
            return panel as T;
        }

        public bool PopPanel()
        {
            if (_PanelStack.Count == 0)
            {
                return false;
            }
            else
            {
                _PanelStack.Pop().Hide();
                return true;
            }

        }

        public IPanel PeekPanel()
        {
            IPanel panel;
            _PanelStack.TryPeek(out panel);
            return panel;
        }

        private void InitPanelRect(GameObject panelPrefab)
        {
            //初始化UI位置
            panelPrefab.transform.localPosition = Vector3.zero;
            panelPrefab.transform.localScale = Vector3.one;
            (panelPrefab.transform as RectTransform).offsetMax = Vector2.zero;
            (panelPrefab.transform as RectTransform).offsetMin = Vector2.zero;
        }
    }
}


