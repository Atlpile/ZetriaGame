using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePile : MonoBehaviour
{
    private AudioSource FireSource;

    private void Awake()
    {
        FireSource = this.GetComponent<AudioSource>();
        GameManager.Instance.m_EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    private void OnDestroy()
    {
        GameManager.Instance.m_EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().Hurt();
        }
    }

    public void OnUpdateAudioSourceVolume(float volume)
    {
        FireSource.volume = volume;
    }
}
