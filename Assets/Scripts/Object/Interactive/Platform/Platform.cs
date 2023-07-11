using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private AudioSource platformMoveSource;

    private void Awake()
    {
        platformMoveSource = this.GetComponent<AudioSource>();
        GameManager.Instance.EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    public void Move(Transform target, float moveSpeed)
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    public void UpdateAudio(bool isActive)
    {
        platformMoveSource.enabled = isActive;
    }


    private void OnUpdateAudioSourceVolume(float volume)
    {
        platformMoveSource.volume = volume;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        BaseCharacter character = other.gameObject.GetComponent<BaseCharacter>();
        if (character != null)
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        BaseCharacter character = other.gameObject.GetComponent<BaseCharacter>();
        if (character != null)
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }
}
