using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

/*
    额外需求：
        1.可以在物体上添加音源组件，并控制其中的脚本
        2.制作音频淡入淡出功能
*/

public class AudioManager
{
    public float effectVolume = 1;
    public float bgmVolume = 1;

    private Dictionary<string, AudioClip> _AudioContainer = new Dictionary<string, AudioClip>();
    private AudioSource _bgmChannel;
    private AudioSource _effectChannel;
    private GameObject _audioPlayer;


    public AudioManager()
    {
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "AudioPlayer");
        GameManager.Instance.ResourcesLoader.LoadAsync<GameObject>(E_ResourcesPath.Object, "AudioPlayer", (audioPlayer) =>
        {
            _audioPlayer = audioPlayer;
        }, false);
    }


    public void AudioPlay(E_AudioType type, string name, bool isLoop = false, bool usePool = true)
    {
        if (_AudioContainer.ContainsKey(name))
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
                _AudioContainer.Add(name, audioClip);

                PlayAudioClip(type, name, isLoop, usePool);
            });
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
                    _bgmChannel.DOFade(0, 1).OnComplete(() =>
                    {
                        //BUG:不能返回BGM对象至对象池
                        GameManager.Instance.ObjectPoolManager.AddObject(_bgmChannel.gameObject);
                        GameManager.Instance.ObjectPoolManager.ReturnObject(_bgmChannel.gameObject);
                        _bgmChannel = null;
                    });
                    break;
                case E_AudioSettingType.Destroy:
                    _bgmChannel.DOFade(0, 1).OnComplete(() =>
                    {
                        GameObject.Destroy(_bgmChannel.gameObject);
                        _bgmChannel = null;
                    });
                    break;
                case E_AudioSettingType.Pause:
                    _bgmChannel.Pause();
                    break;
                case E_AudioSettingType.Resume:
                    _bgmChannel.UnPause();
                    break;
            }
        }
        else
            Debug.Log("音频为空，操作无效");

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

    private void PlayAudioClip(E_AudioType type, string name, bool isLoop, bool usePool)
    {
        GameObject audioPlayerObj;
        if (usePool)
        {
            audioPlayerObj = GameManager.Instance.ObjectPoolManager.GetObject("AudioPlayer");
        }
        else
            audioPlayerObj = GameObject.Instantiate(_audioPlayer);

        AudioPlayer audioPlayer = audioPlayerObj.GetComponent<AudioPlayer>();
        AudioSource audioPlayerSource = audioPlayerObj.GetComponent<AudioSource>();

        audioPlayerObj.transform.SetParent(GameManager.Instance.transform);
        audioPlayerSource.clip = _AudioContainer[name];

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

    public void Clear()
    {
        _AudioContainer.Clear();
    }

}
