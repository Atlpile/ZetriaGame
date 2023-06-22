using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    额外需求：
        1.可以在物体上添加音源组件，并控制其中的脚本
*/

public class AudioManager
{
    public float effectVolume = 1;
    public float bgmVolume = 1;

    private Dictionary<string, AudioClip> AudioDict;
    private AudioSource BGMChannel;
    private AudioSource EffectChannel;

    public UnityAction AudioSourceVolumeChanged;

    public AudioManager()
    {
        AudioDict = new Dictionary<string, AudioClip>();
    }

    public void AudioPlay(E_AudioType type, string name, bool isLoop = false)
    {
        if (AudioDict.ContainsKey(name))
        {
            GameObject poolObj = GameManager.Instance.m_ObjectPoolManager.GetObject(name);
            PlayAudioClip(type, name, AudioDict[name], poolObj, isLoop);
        }
        else
        {
            //加载音频
            AudioClip audioClip = GameManager.Instance.m_ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, name);
            if (audioClip == null)
            {
                Debug.LogError("AudioManager:未找到该名称的音频：" + name + ",请检查Resources文件夹中的音频是否存在");
                return;
            }

            //创建音乐对象
            GameObject soundObj = new GameObject(name);
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = audioClip;

            //记录音乐对象
            AudioDict.Add(name, audioClip);
            GameManager.Instance.m_ObjectPoolManager.AddObject(soundObj, name);
            GameObject poolObj = GameManager.Instance.m_ObjectPoolManager.GetObject(name);

            //播放音频及设置
            PlayAudioClip(type, name, audioClip, poolObj, isLoop);
        }
    }

    public void BGMSetting(E_AudioSetttingType type)
    {
        if (BGMChannel != null)
        {
            switch (type)
            {
                case E_AudioSetttingType.Stop:
                    GameManager.Instance.m_ObjectPoolManager.ReturnObject(BGMChannel.gameObject);
                    BGMChannel = null;
                    break;
                case E_AudioSetttingType.Pause:
                    BGMChannel.Pause();
                    break;
                case E_AudioSetttingType.Resume:
                    BGMChannel.UnPause();
                    break;
            }
        }
        else
        {
            Debug.Log("音频为空，操作无效");
        }
    }

    public void SetVolume(E_AudioType type, float volume)
    {
        switch (type)
        {
            case E_AudioType.BGM:
                SetBGMVolume(volume);
                break;
            case E_AudioType.Effect:
                SetEffectVolume(volume);
                GameManager.Instance.m_EventManager.EventTrigger(E_EventType.UpdateAudioSourceVolume, volume);
                break;
        }
    }

    public void LoadAudioData()
    {
        SettingData settingData = GameManager.Instance.m_SaveLoadManager.LoadData<SettingData>("SettingData");
        SetVolume(E_AudioType.BGM, settingData.volume_BGM);
        SetVolume(E_AudioType.Effect, settingData.volume_Effect);
    }

    public void Clear()
    {
        AudioDict.Clear();
    }


    private void PlayAudioClip(E_AudioType type, string name, AudioClip audioClip, GameObject audioObj, bool isLoop)
    {
        audioObj.transform.SetParent(GameManager.Instance.transform);

        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        switch (type)
        {
            case E_AudioType.BGM:

                if (BGMChannel != null)
                    BGMSetting(E_AudioSetttingType.Stop);

                audioSource.loop = isLoop;
                audioSource.volume = bgmVolume;
                audioSource.Play();
                BGMChannel = audioSource;
                break;
            case E_AudioType.Effect:
                GameManager.Instance.StartCoroutine(IE_PlayOnceAudio(name, audioClip, audioObj));
                audioSource.loop = isLoop;
                audioSource.volume = effectVolume;
                audioSource.Play();
                EffectChannel = audioSource;
                break;
        }
    }

    private IEnumerator IE_PlayOnceAudio(string name, AudioClip clip, GameObject audioObj)
    {
        yield return new WaitForSeconds(clip.length);
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(audioObj);
    }

    private void SetBGMVolume(float volume)
    {
        if (BGMChannel != null)
            BGMChannel.volume = volume;

        bgmVolume = volume;
    }

    private void SetEffectVolume(float volume)
    {
        if (EffectChannel != null)
            EffectChannel.volume = volume;

        effectVolume = volume;
    }

}
