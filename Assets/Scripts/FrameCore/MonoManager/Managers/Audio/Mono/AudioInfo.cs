using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FrameCore
{
    public class AudioInfo
    {
        public AudioClip audioClip;
        public AudioMixerGroup audioMixerGroup;
        public bool isLoop;
        public bool isMute;
        public bool isPlayOnAwake;
        public float volume;
        public float pitch;

        public AudioInfo(AudioClip audioClip, AudioMixerGroup audioMixerGroup)
        {
            this.audioClip = audioClip;
            this.audioMixerGroup = audioMixerGroup;
        }

        public AudioInfo(bool isLoop, float volume, float pitch)
        {
            this.isLoop = isLoop;
            this.volume = volume;
            this.pitch = pitch;
        }

        public AudioInfo(bool isMute)
        {
            this.isMute = isMute;
        }
    }
}

