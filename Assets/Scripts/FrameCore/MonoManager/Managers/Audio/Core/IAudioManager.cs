using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IAudioManager : IManager
    {
        void AudioPlay(E_AudioType type, string name, bool isLoop = false, float volume = 1f, int channel = 0);
        void SetBGM(E_AudioPlayStatus status, int playChannel);
        void SetAudioGroupVolume(E_VolumeType type, float volume);
    }
}


