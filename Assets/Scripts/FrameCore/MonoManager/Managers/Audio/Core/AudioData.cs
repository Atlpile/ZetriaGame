using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class AudioData
    {
        public float masterVolume;
        public float bgmVolue;
        public float effectVolume;
        public float voiceVolue;

        public AudioData()
        {
            masterVolume = 0.5f;
            bgmVolue = 0.5f;
            effectVolume = 0.5f;
            voiceVolue = 0.5f;
        }
    }
}


