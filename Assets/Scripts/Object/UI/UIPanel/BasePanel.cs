using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


/*
    额外添加功能
    1.在编辑器中拖出预制体到场景时，自动添加设置好的Canvas
    
    功能优化
    1.组件获取优化：在申明变量或属性完成后，自动根据变量或属性名获取UI名称的组件
*/


public abstract class BasePanel : MonoBehaviour
{
    public bool isDuration;
    private Dictionary<string, List<UIBehaviour>> UIComponentDic = new Dictionary<string, List<UIBehaviour>>();
    protected CanvasGroup canvasGroup;
    protected float fadeDuration = 1f;



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

    public virtual void Hide(TweenCallback TweenCompleteCallBack)
    {
        if (TweenCompleteCallBack != null)
            SetTransitionEffect(true, TweenCompleteCallBack);
    }

    public virtual void Show(TweenCallback TweenCompleteCallBack)
    {
        if (TweenCompleteCallBack != null)
            SetTransitionEffect(false, TweenCompleteCallBack);
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

    public void SetPanelInteractiveStatus(bool canInteractive)
    {
        canvasGroup.blocksRaycasts = canInteractive;
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
                UIComponentDic[objName].Add(components[i]);
            else
                UIComponentDic.Add(objName, new List<UIBehaviour>() { components[i] });


            if (components[i] is Button)
                (components[i] as Button).onClick.AddListener(() => { OnClick(objName); });
            else if (components[i] is Toggle)
                (components[i] as Toggle).onValueChanged.AddListener((value) => { OnValueChanged(objName, value); });
            else if (components[i] is Slider)
                (components[i] as Slider).onValueChanged.AddListener((value) => { OnValueChanged(objName, value); });
        }
    }

    protected void SetTransitionEffect(bool isIn, TweenCallback CompleteCallBack)
    {
        if (isIn)
        {
            // Debug.Log("淡入");
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, fadeDuration).OnComplete(CompleteCallBack);
        }
        else
        {
            // Debug.Log("淡出");
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, fadeDuration).OnComplete(CompleteCallBack);
        }
    }

}
