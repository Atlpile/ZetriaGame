using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Slider slider_BGM;
    private Slider slider_Effect;
    private Toggle toggle_FullScreen;
    private Toggle toggle_Bloom;
    private Button button_Close;

    protected override void Awake()
    {
        GetChildrenAllUIComponent<Slider>();
        GetChildrenAllUIComponent<Toggle>();
        GetChildrenAllUIComponent<Button>();

        slider_BGM = GetUIComponent<Slider>("slider_BGM");
        slider_Effect = GetUIComponent<Slider>("slider_Effect");
        toggle_FullScreen = GetUIComponent<Toggle>("toggle_FullScreen");
        toggle_Bloom = GetUIComponent<Toggle>("toggle_Bloom");
        button_Close = GetUIComponent<Button>("button_Close");
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "button_Close":
                CloseSettingPanel();
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, bool value)
    {
        switch (toggleName)
        {
            case "toggle_FullScreen":
                Screen.fullScreen = value;
                break;
            case "toggle_Bloom":
                break;
        }
    }

    protected override void OnValueChanged(string sliderName, float value)
    {
        switch (sliderName)
        {
            case "slider_BGM":
                GameManager.Instance.m_AudioController.SetBGMVolume(value);
                break;
            case "slider_Effect":
                GameManager.Instance.m_AudioController.SetEffectVolume(value);
                break;
        }
    }

    public override void ShowSelf()
    {
        //TODO：加载设置中的数据
    }

    public override void HideSelf()
    {
        //TODO：保存设置的数据
    }

    private void CloseSettingPanel()
    {
        GameManager.Instance.m_UIManager.HidePanel<SettingPanel>();
    }
}
