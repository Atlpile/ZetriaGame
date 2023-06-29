using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/*
    TODO：添加Panel切换动画

*/


public abstract class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> UIComponentDic = new Dictionary<string, List<UIBehaviour>>();
    private CanvasGroup canvasGroup;
    private float fadeDuration = 2f;

    protected virtual void Awake()
    {
        // GetChildrenUIComponent<Button>();
        // GetChildrenUIComponent<Image>();
        // GetChildrenUIComponent<Text>();
        // GetChildrenUIComponent<Toggle>();
        // GetChildrenUIComponent<Slider>();
        // GetChildrenUIComponent<ScrollRect>();
        // GetChildrenUIComponent<InputField>();

        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void Show(TweenCallback ShowCallBack = null)
    {
        if (ShowCallBack != null)
            SetTransitionEffect(E_UITransitionType.Fade, true, ShowCallBack);
    }

    public virtual void Hide(TweenCallback HideCallBack = null)
    {
        if (HideCallBack != null)
            SetTransitionEffect(E_UITransitionType.Fade, false, HideCallBack);
    }


    protected virtual void OnClick(string buttonName)
    {

    }

    protected virtual void OnValueChanged(string toggleName, bool value)
    {

    }

    protected virtual void OnValueChanged(string sliderName, float value)
    {

    }

    public void ClearUIComponentDic()
    {
        UIComponentDic.Clear();
    }

    protected T GetUIComponent<T>(string uiObjName) where T : UIBehaviour
    {
        if (UIComponentDic.ContainsKey(uiObjName))
        {
            for (int i = 0; i < UIComponentDic[uiObjName].Count; ++i)
            {
                if (UIComponentDic[uiObjName][i] is T)
                    return UIComponentDic[uiObjName][i] as T;
            }
        }

        Debug.LogError("该面板中没有名为" + uiObjName + "的组件,请检查UI组件名称是否正确");
        return null;
    }

    protected void GetChildrenAllUIComponent<T>() where T : UIBehaviour
    {
        T[] components = this.GetComponentsInChildren<T>(true);
        for (int i = 0; i < components.Length; i++)
        {
            string objName = components[i].gameObject.name;
            if (UIComponentDic.ContainsKey(objName))
            {
                UIComponentDic[objName].Add(components[i]);
            }
            else
            {
                UIComponentDic.Add(objName, new List<UIBehaviour>() { components[i] });
            }

            if (components[i] is Button)
            {
                (components[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            else if (components[i] is Toggle)
            {
                (components[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
            else if (components[i] is Slider)
            {
                (components[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    protected void SetTransitionEffect(E_UITransitionType type, bool isIn, TweenCallback callback = null)
    {
        switch (type)
        {
            case E_UITransitionType.Fade:
                if (isIn)
                {
                    canvasGroup.alpha = 0;
                    canvasGroup.DOFade(1, fadeDuration).OnComplete(callback);
                    // Debug.Log("淡入");
                }
                else
                {
                    // Debug.Log("淡出");
                    canvasGroup.DOFade(1, fadeDuration).OnComplete(callback);
                }
                break;
        }
    }
}
