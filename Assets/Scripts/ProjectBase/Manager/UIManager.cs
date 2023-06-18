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

            T panel = uiPrefab.GetComponent<T>();
            PanelDic.Add(panelName, panel);
            panel.ShowSelf();
            return panel;
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
            Object.Destroy(PanelDic[panelName].gameObject);
            PanelDic.Remove(panelName);
        }
    }

    public T ShowPanelFromPool<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        //有面板
        if (PanelDic.ContainsKey(panelName))
        {
            GameManager.Instance.m_ObjectPoolManager.GetObject(panelName, rectCanvas);
            Debug.LogWarning("UIManager:场景中存在该Panel");
            return PanelDic[panelName] as T;
        }
        //没有面板
        else
        {
            GameObject poolPrefab = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject(panelName, E_ResourcesPath.UI, rectCanvas);

            poolPrefab.transform.SetParent(rectCanvas);
            poolPrefab.transform.localPosition = Vector3.zero;
            poolPrefab.transform.localScale = Vector3.one;
            (poolPrefab.transform as RectTransform).offsetMax = Vector2.zero;
            (poolPrefab.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = poolPrefab.GetComponent<T>();
            PanelDic.Add(panelName, panel);
            panel.ShowSelf();
            return panel;
        }
    }

    public void HidePanelFromPool<T>() where T : BasePanel
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
            GameManager.Instance.m_ObjectPoolManager.ReturnObject(PanelDic[panelName].gameObject);
            PanelDic.Remove(panelName);
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

    public bool GetExistPanel(string panelName)
    {
        if (PanelDic.ContainsKey(panelName))
            return true;
        else
            return false;
    }

    public void ClearUI()
    {
        PanelDic.Clear();
    }
}
