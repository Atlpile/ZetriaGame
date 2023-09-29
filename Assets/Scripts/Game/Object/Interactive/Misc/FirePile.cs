using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePile : MonoBehaviour
{
    private AudioSource fireSource;

    private void Awake()
    {
        fireSource = this.GetComponent<AudioSource>();
        GameManager.Instance.EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().OnDamage(this.transform.position);
        }
    }

    public void OnUpdateAudioSourceVolume(float volume)
    {
        fireSource.volume = volume;
    }
}
