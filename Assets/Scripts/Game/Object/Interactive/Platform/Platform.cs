using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private AudioSource _platformMoveSource;
    private float _moveSpeed;

    // public event Action<bool> OnUpdateAudio;
    // public event Action OnUpdateMove;

    private void Awake()
    {
        _platformMoveSource = this.GetComponent<AudioSource>();
        GameManager.Instance.EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }


    public void Move(Transform target)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, _moveSpeed * Time.deltaTime);
    }

    public void UpdateAudioPlay(bool isActive)
    {
        _platformMoveSource.enabled = isActive;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        moveSpeed = _moveSpeed;
    }


    private void OnUpdateAudioSourceVolume(float volume)
    {
        _platformMoveSource.volume = volume;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<BaseCharacter>(out var character))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //FIXME:退出时，设置父级在Character里
        if (other.gameObject.TryGetComponent<BaseCharacter>(out var character))
        {
            other.transform.SetParent(null);
        }
    }


}
