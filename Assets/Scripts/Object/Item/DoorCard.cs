using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.m_EventManager.EventTrigger(E_EventType.PickUpDoorCard);
            GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "door_card_collect");
            Destroy(this.gameObject);

        }
    }
}
