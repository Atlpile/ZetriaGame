using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IObject
{
    private AudioSource _audioSource;

    private bool _usePool;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Create();
    }

    private void OnDisable()
    {
        Release();
    }


    public void Play(bool isLoop, float volume, bool usePool)
    {
        SetAudioSourceInfo(isLoop, volume, usePool);
        _audioSource.Play();
    }

    public void PlayOnce(bool isLoop, float volume, bool usePool)
    {
        SetAudioSourceInfo(isLoop, volume, usePool);
        _audioSource.Play();

        if (_audioSource.clip != null)
            StartCoroutine(IE_PlayOnceAudio());
        else
            Debug.LogError("AudioPlayer中没有音频,不能播放音效");
    }

    public void SetAudioSourceInfo(bool isLoop, float volume, bool usePool)
    {
        _audioSource.loop = isLoop;
        _audioSource.volume = volume;
        _usePool = usePool;
    }

    private IEnumerator IE_PlayOnceAudio()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        Hide();
    }


    public void Create()
    {

    }

    public void Hide()
    {
        if (_usePool)
            GameManager.Instance.ObjectPoolManager.ReturnObject(this.gameObject);
        else
            Destroy(this.gameObject);
    }

    public void Release()
    {

    }
}
