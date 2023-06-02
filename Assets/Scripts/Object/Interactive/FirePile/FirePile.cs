using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePile : MonoBehaviour
{
    private AudioSource _fireSource;

    private void Awake()
    {
        _fireSource = this.GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerController>().Hurt();
        }
    }

    private void Update()
    {
        if (_fireSource.volume != GameManager.Instance.m_AudioController.effectVolume)
            _fireSource.volume = GameManager.Instance.m_AudioController.effectVolume;
    }


}
