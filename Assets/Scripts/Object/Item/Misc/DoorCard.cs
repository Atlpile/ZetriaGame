using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpDoorCard);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "collect");
            Destroy(this.gameObject);
        }
    }
}
