using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    public Dictionary<string, BasePanel> PanelDic = new Dictionary<string, BasePanel>();
    public RectTransform rectCanvas;

    private GameObject canvas;
    private GameObject eventSystem;

    public UIManager()
    {
        //创建Canvas
        if (!GameObject.FindGameObjectWithTag("Canvas"))
        {
            canvas = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, "Canvas");
            rectCanvas = canvas.transform as RectTransform;
            GameObject.DontDestroyOnLoad(canvas);
        }

        //创建EventSystem
        if (!GameObject.FindGameObjectWithTag("EventSystem"))
        {
            eventSystem = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, "EventSystem");
            GameObject.DontDestroyOnLoad(eventSystem);
        }

    }

    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
        {
            // //TODO:从对象池中取出
            // GameManager.Instance.m_ObjectPool.GetObject(panelName, rectCanvas);
            Debug.LogWarning("UIManager:场景中存在该Panel");
            return PanelDic[panelName] as T;
        }
        else
        {
            GameObject uiPrefab = GameManager.Instance.m_ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, panelName);


            uiPrefab.transform.SetParent(rectCanvas);
            uiPrefab.transform.localPosition = Vector3.zero;
            uiPrefab.transform.localScale = Vector3.one;
            (uiPrefab.transform as RectTransform).offsetMax = Vector2.zero;
            (uiPrefab.transform as RectTransform).offsetMin = Vector2.zero;

            // //TODO:添加至对象池
            // GameManager.Instance.m_ObjectPool.AddObject(uiPrefab, panelName);

            T panel = uiPrefab.GetComponent<T>();
            PanelDic.Add(panelName, panel);
            panel.ShowSelf();
            return panel;
        }
    }

    public void ShowPanelAsync<T>(UnityAction<T> LoadAction) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager:场景中存在该Panel");
            if (LoadAction != null)
            {
                LoadAction(PanelDic[panelName] as T);
            }
            return;
        }
        else
        {
            GameManager.Instance.m_ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.UI, panelName, (uiPrefab) =>
            {
                uiPrefab.transform.SetParent(rectCanvas);
                uiPrefab.transform.localPosition = Vector3.zero;
                uiPrefab.transform.localScale = Vector3.one;
                (uiPrefab.transform as RectTransform).offsetMax = Vector2.zero;
                (uiPrefab.transform as RectTransform).offsetMin = Vector2.zero;

                T panel = uiPrefab.GetComponent<T>();
                PanelDic.Add(panelName, panel);

                if (LoadAction != null)
                    LoadAction(panel);

                panel.ShowSelf();
            });
        }
    }

    public void HidePanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (!PanelDic.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager:场景中不存在该Panel");
            return;
        }
        else
        {
            PanelDic[panelName].HideSelf();
            //TODO：替换为对象池
            // GameManager.Instance.m_ObjectPool.ReturnObject(panelName, PanelDic[panelName].gameObject);
            Object.Destroy(PanelDic[panelName].gameObject);
            PanelDic.Remove(panelName);
        }
    }

    public T GetExistPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
        {
            return PanelDic[panelName] as T;
        }
        else
        {
            Debug.LogWarning("UIManager:场景中不存在该Panel");
            return null;
        }
    }
}
