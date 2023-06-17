using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController
{
    private AmmoInfo ammoInfo;
    public AmmoInfo AmmoInfo => ammoInfo;


    private GamePanel _GamePanel => GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>();


    public AmmoController()
    {
        ammoInfo = new AmmoInfo();
    }


    public void PickUpPistolAmmoPackage()
    {
        ammoInfo._maxPistolAmmoCount += ammoInfo._packagePistolAmmoCount;
        _GamePanel.UpdatePistolAmmoText(ammoInfo._currentPistolAmmoCount, ammoInfo._maxPistolAmmoCount);

    }

    public void PickUpShotGunAmmoPackage()
    {
        ammoInfo._maxShotGunAmmoCount += ammoInfo._packageShotGunAmmoCount;
        _GamePanel.UpdateShortGunAmmoText(ammoInfo._currentShotGunAmmoCount, ammoInfo._maxShotGunAmmoCount);
    }

    public void ReloadPistolAmmo()
    {
        for (int i = ammoInfo._currentPistolAmmoCount; i < ammoInfo._currentPistolAmmoLimit; i++)
        {
            if (ammoInfo._maxPistolAmmoCount == 0) break;
            ammoInfo._maxPistolAmmoCount--;
            ammoInfo._currentPistolAmmoCount++;
        }
        _GamePanel.UpdatePistolAmmoText(ammoInfo._currentPistolAmmoCount, ammoInfo._maxPistolAmmoCount);
    }

    public void ReloadShotGunAmmo()
    {
        for (int i = ammoInfo._currentShotGunAmmoCount; i < ammoInfo._currentShotGunAmmoLimit; i++)
        {
            if (ammoInfo._maxShotGunAmmoCount == 0) break;
            ammoInfo._maxShotGunAmmoCount--;
            ammoInfo._currentShotGunAmmoCount++;
        }
        _GamePanel.UpdateShortGunAmmoText(ammoInfo._currentShotGunAmmoCount, ammoInfo._maxShotGunAmmoCount);
    }


    public void ClearAllAmmo()
    {
        ammoInfo._currentPistolAmmoCount = 0;
        ammoInfo._currentShotGunAmmoCount = 0;
        _GamePanel.UpdatePistolAmmoText(ammoInfo._currentPistolAmmoCount, ammoInfo._maxPistolAmmoCount);
        _GamePanel.UpdateShortGunAmmoText(ammoInfo._currentShotGunAmmoCount, ammoInfo._maxShotGunAmmoCount);
    }

    public void UsePistolAmmo()
    {
        ammoInfo._currentPistolAmmoCount--;
        _GamePanel.UpdatePistolAmmoText(ammoInfo._currentPistolAmmoCount, ammoInfo._maxPistolAmmoCount);
    }

    public void UseShotGunAmmo()
    {
        ammoInfo._currentShotGunAmmoCount--;
        _GamePanel.UpdateShortGunAmmoText(ammoInfo._currentShotGunAmmoCount, ammoInfo._maxShotGunAmmoCount);
    }
}
