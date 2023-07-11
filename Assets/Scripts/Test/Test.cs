using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        AudioTest();
    }



    private void AudioTest()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.BGM, "bgm_01", true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_fire");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Pause);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Stop);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.AudioManager.BGMSetting(E_AudioSettingType.Resume);
        }
    }


}

