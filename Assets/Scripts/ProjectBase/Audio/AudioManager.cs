using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    额外需求：
        1.可以在物体上添加音源组件，并控制其中的脚本
*/

public class AudioManager
{
    public Dictionary<string, AudioClip> AudioDict;
    private AudioSource BGMChannel;
    private AudioSource EffectChannel;


    public float effectVolume = 1;
    public float bgmVolume;

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
            GameObject resAudioObj = new GameObject(name);
            AudioSource audioSource = resAudioObj.AddComponent<AudioSource>();
            audioSource.clip = audioClip;

            //记录音乐对象
            AudioDict.Add(name, audioClip);
            GameManager.Instance.m_ObjectPoolManager.AddObject(resAudioObj, name);
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
                BGMChannel.volume = volume;
                break;
            case E_AudioType.Effect:
                effectVolume = volume;
                break;
        }
    }


    public void SetBGMVolume(float volume)
    {
        BGMChannel.volume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
    }

    public void LoadAudioData()
    {
        //OPTIMIZE：初始时没有AudioSource，需要先有AudioSource才能修改BGM
        SettingData settingData = GameManager.Instance.m_SaveLoadManager.LoadData<SettingData>("SettingData");
        SetBGMVolume(settingData.volume_BGM);
        SetEffectVolume(settingData.volume_Effect);
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



}
