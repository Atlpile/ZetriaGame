using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameCore
{
    //获取一次面板所有指定组件
    //面板获取单个组件

    public abstract class BaseUIPanel : BaseComponent, IPanel, IPoolObject
    {
        protected CanvasGroup canvasGroup;
        private readonly Dictionary<string, List<UIBehaviour>> UIComponentContainer = new();

        private void Awake()
        {
            if (!TryGetComponent(out canvasGroup))
                canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

            OnGetUIComponent();
        }

        public virtual void Show()
        {

        }

        public virtual void Hide()
        {
            OnReturn();
        }

        protected abstract void OnGetUIComponent();

        protected virtual void OnClick(string buttonName)
        {

        }

        protected virtual void OnValueChanged(string toggleName, bool value)
        {

        }

        protected virtual void OnValueChanged(string sliderName, float value)
        {

        }

        protected T GetUIComponent<T>(string uiObjName) where T : UIBehaviour
        {
            if (UIComponentContainer.ContainsKey(uiObjName))
            {
                for (int i = 0; i < UIComponentContainer[uiObjName].Count; ++i)
                {
                    if (UIComponentContainer[uiObjName][i] is T)
                        return UIComponentContainer[uiObjName][i] as T;
                }
            }
            Debug.LogError("该面板中没有名为" + uiObjName + "的组件,请检查UI组件名称是否正确");
            return null;
        }

        protected void GetChildAllUIComponent<T>() where T : UIBehaviour
        {
            T[] components = this.GetComponentsInChildren<T>(true);
            for (int i = 0; i < components.Length; i++)
            {
                string objName = components[i].gameObject.name;
                if (UIComponentContainer.ContainsKey(objName))
                    UIComponentContainer[objName].Add(components[i]);
                else
                    UIComponentContainer.Add(objName, new List<UIBehaviour>() { components[i] });

                if (components[i] is Button)
                    (components[i] as Button).onClick.AddListener(() => { OnClick(objName); });
                else if (components[i] is Toggle)
                    (components[i] as Toggle).onValueChanged.AddListener((value) => { OnValueChanged(objName, value); });
                else if (components[i] is Slider)
                    (components[i] as Slider).onValueChanged.AddListener((value) => { OnValueChanged(objName, value); });
            }
        }

        public virtual void OnInit()
        {

        }

        public virtual void OnCreate()
        {

        }

        public virtual void OnRelease()
        {

        }

        public void OnReturn()
        {
            Manager.GetManager<ObjectPoolManager>().ReturnObject(this.gameObject);
        }
    }
}


