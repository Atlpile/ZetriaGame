using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class AudioChannel
    {
        public Dictionary<int, AudioPlayer> BGMChannel = new();        //BGM频道
        public Dictionary<int, AudioPlayer> EffectChannel = new();      //声音频道
        public Dictionary<int, AudioPlayer> VoiceChannel = new();      //人声频道

        public void AddPlayer(E_AudioType type, AudioPlayer audioPlayer, int channel)
        {
            switch (type)
            {
                case E_AudioType.Effect:
                    EffectChannel.Add(channel, audioPlayer);
                    break;
                case E_AudioType.Voice:
                    VoiceChannel.Add(channel, audioPlayer);
                    break;
                case E_AudioType.BGM:
                    BGMChannel.Add(channel, audioPlayer);
                    break;
            }
        }

        public void RemovePlayer(E_AudioType type, int channel)
        {
            switch (type)
            {
                case E_AudioType.Effect:
                    EffectChannel.Remove(channel);
                    break;
                case E_AudioType.BGM:
                    BGMChannel.Remove(channel);
                    break;
                case E_AudioType.Voice:
                    VoiceChannel.Remove(channel);
                    break;
            }
        }


    }
}


