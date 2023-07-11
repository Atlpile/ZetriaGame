using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpShortGun);
            GameManager.Instance.ItemManager.GetShotGun();
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_ohYeah");
            Destroy(this.gameObject);
        }
    }
}
