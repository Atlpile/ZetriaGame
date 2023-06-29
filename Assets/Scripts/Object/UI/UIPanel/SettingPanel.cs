using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        base.Awake();

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
                GameManager.Instance.m_AudioManager.SetVolume(E_AudioType.BGM, value);
                break;
            case "slider_Effect":
                GameManager.Instance.m_AudioManager.SetVolume(E_AudioType.Effect, value);
                break;
        }
    }

    public override void Show(TweenCallback ShowCallBack = null)
    {
        LoadSettingData();
    }

    public override void Hide(TweenCallback callback = null)
    {
        SaveSettingData();
    }

    private void CloseSettingPanel()
    {
        GameManager.Instance.m_UIManager.HidePanel<SettingPanel>();
    }

    public void LoadSettingData()
    {
        SettingData settingData = GameManager.Instance.m_SaveLoadManager.LoadData<SettingData>("SettingData");

        slider_BGM.value = settingData.volume_BGM;
        slider_Effect.value = settingData.volume_Effect;
    }

    private void SaveSettingData()
    {
        SettingData settingData = new SettingData();

        settingData.volume_BGM = slider_BGM.value;
        settingData.volume_Effect = slider_Effect.value;

        GameManager.Instance.m_SaveLoadManager.SaveData(settingData, "SettingData");
    }
}
