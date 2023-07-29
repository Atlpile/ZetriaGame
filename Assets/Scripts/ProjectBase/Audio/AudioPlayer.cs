using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IObject
{
    private AudioSource _audioSource;
    private float volume;

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


    public void Play(bool isLoop, float volume)
    {
        SetAudioSourceInfo(isLoop, volume);
        _audioSource.Play();
    }

    public void SetAudioSourceInfo(bool isLoop, float volume)
    {
        _audioSource.loop = isLoop;
        _audioSource.volume = volume;
    }

    public void PlayOnce(bool isLoop, float volume)
    {
        SetAudioSourceInfo(isLoop, volume);
        _audioSource.Play();

        if (_audioSource.clip != null)
            StartCoroutine(IE_PlayOnceAudio());
        else
            Debug.LogError("AudioPlayer中没有音频,不能播放音效");
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
        // GameManager.Instance.ObjectPoolManager.ReturnObject(this.gameObject);
        Destroy(this.gameObject);
    }

    public void Release()
    {

    }
}
