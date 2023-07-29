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
    private GameObject _audioPlayer;
    private GameObject _audioPlayerPoolObj;

    public AudioManager()
    {
        GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.Object, "AudioPlayer", (audioPlayer) =>
        {
            _audioPlayer = audioPlayer;
        }, false);
    }

    public void AudioPlay(E_AudioType type, string name, bool isLoop = false, bool usePool = true)
    {
        if (AudioDict.ContainsKey(name))
        {
            PlayAudioClip(type, name, isLoop, usePool);
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
                AudioDict.Add(name, audioClip);

                PlayAudioClip(type, name, isLoop, usePool);
            });
        }
    }

    private void PlayAudioClip(E_AudioType type, string name, bool isLoop, bool usePool)
    {
        GameObject audioPlayerObj;
        if (usePool)
            audioPlayerObj = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("AudioPlayer", E_ResourcesPath.Object);
        else
            audioPlayerObj = GameObject.Instantiate(_audioPlayer);

        AudioPlayer audioPlayer = audioPlayerObj.GetComponent<AudioPlayer>();
        AudioSource audioPlayerSource = audioPlayerObj.GetComponent<AudioSource>();

        audioPlayerObj.transform.SetParent(GameManager.Instance.transform);
        audioPlayerSource.clip = AudioDict[name];

        switch (type)
        {
            case E_AudioType.BGM:
                if (_bgmChannel != null)
                    if (usePool)
                        BGMSetting(E_AudioSettingType.Stop);
                    else
                        BGMSetting(E_AudioSettingType.Destroy);

                audioPlayer.Play(isLoop, bgmVolume, usePool);
                _bgmChannel = audioPlayerSource;
                break;
            case E_AudioType.Effect:
                audioPlayer.PlayOnce(isLoop, effectVolume, usePool);
                _effectChannel = audioPlayerSource;
                break;
        }
    }

    public void LoadAudioData()
    {
        SettingData settingData = GameManager.Instance.SaveLoadManager.LoadData<SettingData>("SettingData");
        SetVolume(E_AudioType.BGM, settingData.volume_BGM);
        SetVolume(E_AudioType.Effect, settingData.volume_Effect);
    }

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
