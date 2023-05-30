using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Slider slider_Volume;
    private Slider slider_Exposure;
    private Toggle toggle_FullScreen;
    private Toggle toggle_Bloom;
    private Button button_Close;

    protected override void Awake()
    {
        GetChildrenAllUIComponent<Slider>();
        GetChildrenAllUIComponent<Toggle>();
        GetChildrenAllUIComponent<Button>();

        slider_Volume = GetUIComponent<Slider>("slider_Volume");
        slider_Exposure = GetUIComponent<Slider>("slider_Exposure");
        toggle_FullScreen = GetUIComponent<Toggle>("toggle_FullScreen");
        toggle_Bloom = GetUIComponent<Toggle>("toggle_Bloom");
        button_Close = GetUIComponent<Button>("button_Close");
    }

    protected override void OnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "toggle_FullScreen":
                break;
            case "toggle_Bloom":
                break;
            case "button_Close":
                CloseSettingPanel();
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
