using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FrameCore
{
    public sealed class AudioManager : BaseManager, IAudioManager
    {
        private Dictionary<E_VolumeType, string> _ExposedParamterDic;
        private Dictionary<E_AudioMixerGroupType, AudioMixerGroup> _GroupContainer = new();
        private Dictionary<string, AudioInfo> _AudioInfoContainer = new();
        private Dictionary<int, AudioPlayer> _BGMChannel = new();
        private AudioMixer _audioMixer;

        public IResourcesManager ResourcesManager => Manager.GetManager<IResourcesManager>();
        public IObjectPoolManager ObjectPoolManager => Manager.GetManager<IObjectPoolManager>();

        public AudioManager(MonoManager manager) : base(manager)
        {
            //TODO:在Manager中可以自定义添加AudioMixer
        }

        protected override void OnInit()
        {
            _ExposedParamterDic = new Dictionary<E_VolumeType, string>
            {
               {E_VolumeType.MasterVolume,  "MasterVolume"},
               {E_VolumeType.BGMVolume,     "BGMVolume"},
               {E_VolumeType.EffectVolume,  "EffectVolume"},
               {E_VolumeType.VoiceVolume,   "VoiceVolume"},
            };

            //TODO:提示需加载指定的AudioPlayer和AudioMixer，否则使用默认的或创建新的
            ObjectPoolManager.AddObject_FromResourcesAsync(E_ResourcesPath.PoolObject, "AudioPlayer");
            ResourcesManager.LoadAssetAsync<AudioMixer>(E_ResourcesPath.Misc, "MasterMixer", audioMixer =>
            {
                //获取AudioMixer的所有Groups
                AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");
                foreach (var group in audioMixerGroups)
                {
                    //容器中添加单个Grouop
                    E_AudioMixerGroupType type = (E_AudioMixerGroupType)Enum.Parse(typeof(E_AudioMixerGroupType), group.name);
                    _GroupContainer.Add(type, group);
                }
            });
        }

        public void AudioPlay(E_AudioType type, string name, bool isLoop = false, float volume = 1f, int channel = 0)
        {
            if (_BGMChannel.ContainsKey(channel) && type == E_AudioType.BGM)
                SetBGM(E_AudioPlayStatus.Stop, channel);

            AudioPlayer audioPlayer = ObjectPoolManager.GetObject("AudioPlayer").GetComponent<AudioPlayer>();
            if (_AudioInfoContainer.ContainsKey(name))
            {
                audioPlayer.SetAudioInfo(_AudioInfoContainer[name]);
                audioPlayer.PlayAudio(isLoop, volume);
            }
            else
            {
                ResourcesManager.LoadAssetAsync<AudioClip>(E_ResourcesPath.Audio, name, (audioclip) =>
                {
                    AudioInfo info = new AudioInfo(audioclip, GetAudioMixerGroup(type));
                    audioPlayer.SetAudioInfo(info);
                    audioPlayer.PlayAudio(isLoop, volume);
                    _AudioInfoContainer.Add(name, info);
                });
            }

            if (type == E_AudioType.BGM)
                _BGMChannel.Add(channel, audioPlayer);
        }

        //播放游戏对象上的Audio
        public void PlaySceneAudio(E_AudioType type, AudioPlayer audioPlayer, bool isLoop, float volume)
        {
            audioPlayer.PlayAudio(isLoop, volume);
        }

        public void SetBGM(E_AudioPlayStatus status, int playChannel)
        {
            if (_BGMChannel.ContainsKey(playChannel))
            {
                switch (status)
                {
                    case E_AudioPlayStatus.Stop:
                        _BGMChannel[playChannel].Stop();
                        _BGMChannel.Remove(playChannel);
                        break;
                    case E_AudioPlayStatus.Pause:
                        _BGMChannel[playChannel].Pause();
                        break;
                    case E_AudioPlayStatus.Resume:
                        _BGMChannel[playChannel].Resume();
                        break;
                    case E_AudioPlayStatus.FadeIn:
                        _BGMChannel[playChannel].FadeIn(() => _BGMChannel.Remove(playChannel));
                        break;
                }
            }
        }

        public void SetAudioGroupVolume(E_VolumeType type, float volume)
        {
            float climpVolume = Mathf.Clamp(volume, 0.0001f, 1f);
            _audioMixer.SetFloat(_ExposedParamterDic[type], NormalizeToMixerVolume(climpVolume));
            Debug.Log(NormalizeToMixerVolume(climpVolume));
        }

        public float GetAudioGroupVolume(E_VolumeType type)
        {
            float volume;
            _audioMixer.GetFloat(_ExposedParamterDic[type], out volume);
            Debug.Log(volume);
            return volume;
        }

        public void RemovePlayer()
        {
            foreach (var bgm in _BGMChannel.Keys)
            {
                _BGMChannel[bgm].Stop();
                _BGMChannel.Remove(bgm);
            }
        }

        private float NormalizeToMixerVolume(float volume)
        {
            return Mathf.Log10(volume) * 20;
        }

        private float MixerVolumeToNormalized(float volume)
        {
            return Mathf.Pow(10, (volume) / 20);
        }

        private AudioMixerGroup GetAudioMixerGroup(E_AudioType type)
        {
            AudioMixerGroup group;
            switch (type)
            {
                case E_AudioType.Effect:
                    group = _GroupContainer[E_AudioMixerGroupType.EffectGroup];
                    break;
                case E_AudioType.BGM:
                    group = _GroupContainer[E_AudioMixerGroupType.BGMGroup];
                    break;
                case E_AudioType.Voice:
                    group = _GroupContainer[E_AudioMixerGroupType.VoiceGroup];
                    break;
                default:
                    group = null;
                    break;
            }
            return group;
        }
    }
}


