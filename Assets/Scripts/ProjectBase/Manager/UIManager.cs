using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager
{
    //TODO：使用栈结构存储面板（作用：按ESC键时隐藏最上面的面板）
    private Dictionary<string, BasePanel> PanelDic = new Dictionary<string, BasePanel>();
    private RectTransform rectCanvas;
    private GameObject canvas;
    private GameObject eventSystem;

    public UIManager()
    {
        //创建Canvas
        if (!GameObject.FindGameObjectWithTag("Canvas"))
        {
            canvas = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, "Canvas");
            rectCanvas = canvas.transform as RectTransform;
            GameObject.DontDestroyOnLoad(canvas);
        }

        //创建EventSystem
        if (!GameObject.FindGameObjectWithTag("EventSystem"))
        {
            eventSystem = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, "EventSystem");
            GameObject.DontDestroyOnLoad(eventSystem);
        }
    }


    #region Not Async

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="hasTween">是否启用过渡效果</param>
    /// <param name="TweenEndFunc">过渡完成后执行的内容（需配合hasEffect）</param>
    /// <typeparam name="T">面板类对象</typeparam>
    /// <returns>面板类对象</returns>
    public T ShowPanel<T>(bool hasTween = false, UnityAction TweenEndFunc = null) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager:场景中存在该Panel");
            return PanelDic[panelName] as T;
        }
        else
        {
            GameObject uiPrefab = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, panelName);

            uiPrefab.transform.SetParent(rectCanvas);
            uiPrefab.transform.localPosition = Vector3.zero;
            uiPrefab.transform.localScale = Vector3.one;
            (uiPrefab.transform as RectTransform).offsetMax = Vector2.zero;
            (uiPrefab.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = uiPrefab.GetComponent<T>();
            PanelDic.Add(panelName, panel);

            if (hasTween)
                PanelDic[panelName].Show(() => { TweenEndFunc?.Invoke(); });
            else
                PanelDic[panelName].Show();

            return panel;
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="hasTween">是否显示特效</param>
    /// <param name="TweenEndFunc">特效显示结束时执行的内容（需配合hasEffect）</param>
    /// <typeparam name="T">面板类</typeparam>
    public void HidePanel<T>(bool hasTween = false, UnityAction TweenEndFunc = null) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (!PanelDic.ContainsKey(panelName))
        {
            Debug.LogWarning("UIManager:场景中不存在该Panel");
            return;
        }
        else
        {
            if (hasTween)
            {
                PanelDic[panelName].Hide(() =>
                {
                    Object.Destroy(PanelDic[panelName].gameObject);
                    PanelDic.Remove(panelName);
                    TweenEndFunc?.Invoke();
                });
            }
            else
            {
                PanelDic[panelName].Hide();
                Object.Destroy(PanelDic[panelName].gameObject);
                PanelDic.Remove(panelName);
            }
        }
    }

    public T ShowPanelFromPool<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        //有面板
        if (PanelDic.ContainsKey(panelName))
        {
            GameManager.Instance.ObjectPoolManager.GetObject(panelName, rectCanvas);
            Debug.LogWarning("UIManager:场景中存在该Panel");
            return PanelDic[panelName] as T;
        }
        //没有面板
        else
        {
            GameObject poolPrefab = GameManager.Instance.ObjectPoolManager.GetOrLoadObject(panelName, E_ResourcesPath.UI, rectCanvas);

            poolPrefab.transform.SetParent(rectCanvas);
            poolPrefab.transform.localPosition = Vector3.zero;
            poolPrefab.transform.localScale = Vector3.one;
            (poolPrefab.transform as RectTransform).offsetMax = Vector2.zero;
            (poolPrefab.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = poolPrefab.GetComponent<T>();
            PanelDic.Add(panelName, panel);
            panel.Show();
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
            PanelDic[panelName].Hide();
            GameManager.Instance.ObjectPoolManager.ReturnObject(PanelDic[panelName].gameObject);
            PanelDic.Remove(panelName);
        }
    }

    #endregion


    #region Async

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
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.UI, panelName, (uiPrefab) =>
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

                panel.Show();
            });
        }
    }

    #endregion

    public T GetExistPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
        {
            return PanelDic[panelName] as T;
        }
        else
        {
            Debug.LogWarning("UIManager:场景中不存在" + panelName);
            return null;
        }
    }

    public void Clear()
    {
        PanelDic.Clear();
    }

    public void ClearExistPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (PanelDic.ContainsKey(panelName))
            PanelDic.Remove(panelName);
        else
            Debug.Log("不存在该面板，不能从字典清除");
    }
}
