using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FrameCore
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : BaseComponent, IPoolObject
    {
        private AudioSource _audioSource;
        public override IGameStructure GameStructure { get; }

        private void Awake()
        {
            _audioSource = this.GetComponent<AudioSource>();
        }

        public void SetAudioInfo(AudioInfo info)
        {
            _audioSource.clip = info.audioClip;
            _audioSource.outputAudioMixerGroup = info.audioMixerGroup;
        }

        public void PlayAudio(bool isLoop, float volume)
        {
            _audioSource.volume = volume;

            if (isLoop)
            {
                _audioSource.loop = true;
                _audioSource.Play();
            }
            else
            {
                _audioSource.loop = false;
                _audioSource.PlayOneShot(_audioSource.clip);
                StartCoroutine(IE_PlayOnce());
            }
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public void Resume()
        {
            _audioSource.UnPause();
        }

        public void Stop()
        {
            _audioSource.Stop();
            OnReturn();
        }

        public void FadeIn(Action FadeCompleteAction)
        {
            StartCoroutine(IE_FadeIn(FadeCompleteAction));
        }

        private IEnumerator IE_PlayOnce()
        {
            yield return new WaitForSeconds(_audioSource.clip.length);
            OnReturn();
        }

        private IEnumerator IE_FadeIn(Action FadeCompleteAction)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("过渡1s后返回至对象池");
            FadeCompleteAction?.Invoke();
            OnReturn();
        }

        public void OnInit()
        {

        }

        public void OnCreate()
        {

        }

        public void OnReturn()
        {
            Manager.GetManager<IObjectPoolManager>().ReturnObject(this.gameObject);
        }

        public void OnRelease()
        {

        }


    }
}


