using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using UnityEngine.UI;

namespace Zetria
{
    public class SettingUIPanel : BaseUIPanel
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private Slider slider_BGM;
        private Slider slider_Effect;
        private Toggle toggle_FullScreen;
        private Toggle toggle_Bloom;
        private Button button_Close;



        protected override void OnGetUIComponent()
        {
            GetChildAllUIComponent<Slider>();
            GetChildAllUIComponent<Toggle>();
            GetChildAllUIComponent<Button>();

            slider_BGM = GetUIComponent<Slider>(nameof(slider_BGM));
            slider_Effect = GetUIComponent<Slider>(nameof(slider_Effect));
            toggle_FullScreen = GetUIComponent<Toggle>(nameof(toggle_FullScreen));
            toggle_Bloom = GetUIComponent<Toggle>(nameof(toggle_Bloom));
            button_Close = GetUIComponent<Button>(nameof(button_Close));
        }

        protected override void OnClick(string buttonName)
        {
            switch (buttonName)
            {
                case nameof(button_Close):
                    Manager.GetManager<IUIManager>().HidePanel<SettingUIPanel>();
                    break;
            }
        }

        protected override void OnValueChanged(string toggleName, bool value)
        {
            switch (toggleName)
            {
                case nameof(toggle_FullScreen):
                    Screen.fullScreen = value;
                    break;
                case nameof(toggle_Bloom):
                    break;
            }
        }

        protected override void OnValueChanged(string sliderName, float value)
        {
            var AudioManager = Manager.GetManager<IAudioManager>();
            switch (sliderName)
            {
                case nameof(slider_BGM):
                    AudioManager.SetAudioGroupVolume(E_VolumeType.BGMVolume, value);
                    // GameManager.Instance.AudioManager.SetVolume(E_AudioType.BGM, value);
                    break;
                case nameof(slider_Effect):
                    AudioManager.SetAudioGroupVolume(E_VolumeType.EffectVolume, value);
                    // GameManager.Instance.AudioManager.SetVolume(E_AudioType.Effect, value);
                    break;
            }
        }
    }
}


