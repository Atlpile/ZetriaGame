using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FrameCore
{
    public class AudioMixerConfiger
    {
        public Dictionary<E_VolumeType, string> ExposedParamter;
        public Dictionary<E_AudioMixerGroupType, AudioMixerGroup> GroupContainer = new();
        public AudioMixer audioMixer;
        public AudioMixerGroup[] audioMixerGroups;

        public AudioMixerConfiger()
        {
            ExposedParamter = new Dictionary<E_VolumeType, string>
            {
               {E_VolumeType.MasterVolume,  "MasterVolume"},
               {E_VolumeType.BGMVolume,     "BGMVolume"},
               {E_VolumeType.EffectVolume,  "EffectVolume"},
               {E_VolumeType.VoiceVolume,   "VoiceVolume"},
            };
        }

        public void GetAudioMixerGroups(AudioMixer audioMixer)
        {
            this.audioMixer = audioMixer;
            //获取AudioMixer
            audioMixerGroups = audioMixer.FindMatchingGroups("Master");
            foreach (var group in audioMixerGroups)
            {
                E_AudioMixerGroupType type = (E_AudioMixerGroupType)Enum.Parse(typeof(E_AudioMixerGroupType), group.name);
                GroupContainer.Add(type, group);
            }
        }
    }
}


