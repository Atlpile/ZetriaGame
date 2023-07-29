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

    private Dictionary<string, AudioClip> AudioDict = new Dictionary<string, AudioClip>();
    private AudioSource _bgmChannel;
    private AudioSource _effectChannel;



    #region NoPool

    public void AudioPlay_NoPool(E_AudioType type, string name, bool isLoop = false, float volume = 1f)
    {
        if (AudioDict.ContainsKey(name))
        {
            GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.Object, "AudioPlayer", (audioPlayer) =>
            {
                audioPlayer.name = name;
                audioPlayer.GetComponent<AudioSource>().clip = AudioDict[name];
                PlayAudioClip(type, name, audioPlayer, isLoop, volume);
            });
        }
        else
        {
            //加载音频
            GameManager.Instance.ResourcesLoader.LoadAsync<AudioClip>(E_ResourcesPath.Audio, name, (audioClip) =>
            {
                //判断音频是否为空
                if (audioClip == null)
                {
                    Debug.LogError("AudioManager:未找到该名称的音频：" + name + ",请检查Resources文件夹中的音频是否存在");
                    return;
                }
                AudioDict.Add(name, audioClip);

                //创建新AudioPlayer
                GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.Object, "AudioPlayer", (audioPlayer) =>
                {
                    audioPlayer.name = name;
                    audioPlayer.GetComponent<AudioSource>().clip = audioClip;
                    PlayAudioClip(type, name, audioPlayer, isLoop, volume);
                });
            });
        }
    }

    #endregion

    #region Pool

    public void AudioPlay(E_AudioType type, string name, bool isLoop = false)
    {
        if (AudioDict.ContainsKey(name))
        {
            GameObject poolObj = GameManager.Instance.ObjectPoolManager.GetObject(name);
            PlayAudioClip(type, name, AudioDict[name], poolObj, isLoop);
        }
        else
        {
            //加载音频
            GameManager.Instance.ResourcesLoader.LoadAsync<AudioClip>(E_ResourcesPath.Audio, name, (audioClip) =>
            {
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
                GameManager.Instance.ObjectPoolManager.AddObject(soundObj, name);
                GameObject poolObj = GameManager.Instance.ObjectPoolManager.GetObject(name);

                //播放音频及设置
                PlayAudioClip(type, name, audioClip, poolObj, isLoop);
            });
        }
    }

    #endregion




    public void BGMSetting(E_AudioSettingType type)
    {
        if (_bgmChannel != null)
        {
            switch (type)
            {
                case E_AudioSettingType.Stop:
                    GameManager.Instance.ObjectPoolManager.ReturnObject(_bgmChannel.gameObject);
                    _bgmChannel = null;
                    break;
                case E_AudioSettingType.Pause:
                    _bgmChannel.Pause();
                    break;
                case E_AudioSettingType.Resume:
                    _bgmChannel.UnPause();
                    break;
                case E_AudioSettingType.Destroy:
                    GameObject.Destroy(_bgmChannel.gameObject);
                    _bgmChannel = null;
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

                if (GameManager.Instance.EventManager.GetEventExist(E_EventType.UpdateAudioSourceVolume))
                    GameManager.Instance.EventManager.EventTrigger(E_EventType.UpdateAudioSourceVolume, volume);
                break;
        }
    }

    public void LoadAudioData()
    {
        SettingData settingData = GameManager.Instance.SaveLoadManager.LoadData<SettingData>("SettingData");
        SetVolume(E_AudioType.BGM, settingData.volume_BGM);
        SetVolume(E_AudioType.Effect, settingData.volume_Effect);
    }

    private void PlayAudioClip(E_AudioType type, string name, AudioClip audioClip, GameObject audioObj, bool isLoop)
    {
        audioObj.transform.SetParent(GameManager.Instance.transform);
        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        switch (type)
        {
            case E_AudioType.BGM:
                if (_bgmChannel != null)
                    BGMSetting(E_AudioSettingType.Stop);

                SetAudioSourceInfo(audioSource, isLoop);
                _bgmChannel = audioSource;        //将音频赋给BGMChannel上便于控制
                break;
            case E_AudioType.Effect:
                GameManager.Instance.StartCoroutine(IE_PlayOnceAudio(name, audioClip, audioObj));
                SetAudioSourceInfo(audioSource, isLoop);
                _effectChannel = audioSource;      //将音频赋给EffectChannel上便于控制
                break;
        }
    }

    private void PlayAudioClip(E_AudioType type, string name, GameObject audioPlayer, bool isLoop, float volume)
    {
        audioPlayer.transform.SetParent(GameManager.Instance.transform);
        switch (type)
        {
            case E_AudioType.BGM:
                if (_bgmChannel != null) BGMSetting(E_AudioSettingType.Stop);
                audioPlayer.GetComponent<AudioPlayer>().Play(isLoop, volume);
                _bgmChannel = audioPlayer.GetComponent<AudioSource>();
                break;
            case E_AudioType.Effect:
                audioPlayer.GetComponent<AudioPlayer>().PlayOnce(isLoop, effectVolume);
                _effectChannel = audioPlayer.GetComponent<AudioSource>();
                break;
        }
    }

    private IEnumerator IE_PlayOnceAudio(string name, AudioClip clip, GameObject audioObj)
    {
        yield return new WaitForSeconds(clip.length);
        GameManager.Instance.ObjectPoolManager.ReturnObject(audioObj);
    }

    private void SetAudioSourceInfo(AudioSource audioSource, bool isLoop)
    {
        audioSource.loop = isLoop;
        audioSource.volume = effectVolume;
        audioSource.Play();
    }

    private void SetBGMVolume(float volume)
    {
        if (_bgmChannel != null)
            _bgmChannel.volume = volume;

        bgmVolume = volume;
    }

    private void SetEffectVolume(float volume)
    {
        if (_effectChannel != null)
            _effectChannel.volume = volume;

        effectVolume = volume;
    }

    public void Clear()
    {
        AudioDict.Clear();
    }
}
