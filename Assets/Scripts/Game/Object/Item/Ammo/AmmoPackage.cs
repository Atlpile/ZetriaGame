using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackage : MonoBehaviour
{
    public E_AmmoType ammoType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            switch (ammoType)
            {
                case E_AmmoType.Pistol:
                    GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpPistolAmmo);
                    break;
                case E_AmmoType.ShotGun:
                    GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpShortGunAmmo);
                    break;
            }

            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_ammo_collect");
            Destroy(this.gameObject);
        }
    }
}
