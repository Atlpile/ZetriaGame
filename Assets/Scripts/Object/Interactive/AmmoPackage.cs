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
                    GameManager.Instance.m_AmmoManager.PickUpPistolAmmoPackage();
                    break;
                case E_AmmoType.ShotGun:
                    GameManager.Instance.m_AmmoManager.PickUpShotGunAmmoPackage();
                    break;
            }

            GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "pistol_ammo_collect");
            Destroy(this.gameObject);
        }
    }
}
