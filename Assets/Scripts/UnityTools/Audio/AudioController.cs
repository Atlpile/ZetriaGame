using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController
{
    public Dictionary<string, AudioClip> AudioDict;
    private AudioSource BGMSource;
    private AudioSource EffectSource;


    public float effectVolume = 1;
    public float bgmVolume;

    public AudioController()
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
            AudioClip audioClip = GameManager.Instance.m_ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, name);
            if (audioClip == null)
            {
                Debug.LogError("AudioManager:未找到该名称的音频：" + name + ",请检查Resources文件夹中的音频是否存在");
                return;
            }

            //创建新的AudioObject
            GameObject resAudioObj = new GameObject(name);
            AudioSource audioSource = resAudioObj.AddComponent<AudioSource>();
            audioSource.clip = audioClip;

            AudioDict.Add(name, audioClip);
            GameManager.Instance.m_ObjectPoolManager.AddObject(resAudioObj, name);
            GameObject poolObj = GameManager.Instance.m_ObjectPoolManager.GetObject(name);

            PlayAudioClip(type, name, audioClip, poolObj, isLoop);
        }
    }

    public void BGMStop()
    {
        if (BGMSource != null)
        {
            GameManager.Instance.m_ObjectPoolManager.ReturnObject(BGMSource.gameObject);
            BGMSource = null;
        }
        else
        {
            Debug.LogWarning("音频为空，停止播放音频无效");
        }
    }

    public void BGMPause()
    {
        if (BGMSource != null)
            BGMSource.Pause();
        else
            Debug.LogWarning("音频为空，暂停播放音频无效");

    }

    public void BGMResume()
    {
        if (BGMSource != null)
            BGMSource.UnPause();
        else
            Debug.LogWarning("音频为空，恢复播放音频无效");
    }

    public void SetBGMVolume(float volume)
    {
        BGMSource.volume = volume;
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

                if (BGMSource != null)
                    BGMStop();

                audioSource.loop = isLoop;
                audioSource.Play();
                BGMSource = audioSource;
                break;
            case E_AudioType.Effect:
                GameManager.Instance.StartCoroutine(IE_PlayOnceAudio(name, audioClip, audioObj));
                audioSource.loop = isLoop;
                audioSource.volume = effectVolume;
                audioSource.Play();
                EffectSource = audioSource;
                break;
        }
    }

    private IEnumerator IE_PlayOnceAudio(string name, AudioClip clip, GameObject audioObj)
    {
        yield return new WaitForSeconds(clip.length);
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(audioObj);
    }



}
