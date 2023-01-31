using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> UIComponentDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        // GetChildrenUIComponent<Button>();
        // GetChildrenUIComponent<Image>();
        // GetChildrenUIComponent<Text>();
        // GetChildrenUIComponent<Toggle>();
        // GetChildrenUIComponent<Slider>();
        // GetChildrenUIComponent<ScrollRect>();
        // GetChildrenUIComponent<InputField>();
    }

    public virtual void ShowSelf()
    {

    }

    public virtual void HideSelf()
    {

    }

    protected virtual void OnClick(string buttonName)
    {

    }

    protected virtual void OnValueChanged(string toggleName, bool value)
    {

    }

    protected T GetUIComponent<T>(string componentName) where T : UIBehaviour
    {
        if (UIComponentDic.ContainsKey(componentName))
        {
            for (int i = 0; i < UIComponentDic[componentName].Count; ++i)
            {
                if (UIComponentDic[componentName][i] is T)
                    return UIComponentDic[componentName][i] as T;
            }
        }

        Debug.LogError("该面板中没有名为" + componentName + "的组件,请检查UI组件名称是否正确");
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
        }
    }

    public void ClearUIComponentDic()
    {
        UIComponentDic.Clear();
    }
}
