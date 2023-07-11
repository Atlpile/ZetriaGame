using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController
{
    private AmmoInfo ammoInfo;
    public AmmoInfo AmmoInfo => ammoInfo;


    private GamePanel _GamePanel => GameManager.Instance.UIManager.GetExistPanel<GamePanel>();


    public AmmoController()
    {
        ammoInfo = new AmmoInfo();
    }


    public void PickUpPistolAmmoPackage()
    {
        ammoInfo.maxPistolAmmoCount += ammoInfo.packagePistolAmmoCount;
        _GamePanel.UpdatePistolAmmoText(ammoInfo.currentPistolAmmoCount, ammoInfo.maxPistolAmmoCount);

    }

    public void PickUpShotGunAmmoPackage()
    {
        ammoInfo.maxShotGunAmmoCount += ammoInfo.packageShotGunAmmoCount;
        _GamePanel.UpdateShortGunAmmoText(ammoInfo.currentShotGunAmmoCount, ammoInfo.maxShotGunAmmoCount);
    }

    public void ReloadPistolAmmo()
    {
        for (int i = ammoInfo.currentPistolAmmoCount; i < ammoInfo.currentPistolAmmoLimit; i++)
        {
            if (ammoInfo.maxPistolAmmoCount == 0) break;
            ammoInfo.maxPistolAmmoCount--;
            ammoInfo.currentPistolAmmoCount++;
        }
        _GamePanel.UpdatePistolAmmoText(ammoInfo.currentPistolAmmoCount, ammoInfo.maxPistolAmmoCount);
    }

    public void ReloadShotGunAmmo()
    {
        for (int i = ammoInfo.currentShotGunAmmoCount; i < ammoInfo.currentShotGunAmmoLimit; i++)
        {
            if (ammoInfo.maxShotGunAmmoCount == 0) break;
            ammoInfo.maxShotGunAmmoCount--;
            ammoInfo.currentShotGunAmmoCount++;
        }
        _GamePanel.UpdateShortGunAmmoText(ammoInfo.currentShotGunAmmoCount, ammoInfo.maxShotGunAmmoCount);
    }


    public void ClearAllAmmo()
    {
        ammoInfo.currentPistolAmmoCount = 0;
        ammoInfo.currentShotGunAmmoCount = 0;
        _GamePanel.UpdatePistolAmmoText(ammoInfo.currentPistolAmmoCount, ammoInfo.maxPistolAmmoCount);
        _GamePanel.UpdateShortGunAmmoText(ammoInfo.currentShotGunAmmoCount, ammoInfo.maxShotGunAmmoCount);
    }

    public void UsePistolAmmo()
    {
        ammoInfo.currentPistolAmmoCount--;
        _GamePanel.UpdatePistolAmmoText(ammoInfo.currentPistolAmmoCount, ammoInfo.maxPistolAmmoCount);
    }

    public void UseShotGunAmmo()
    {
        ammoInfo.currentShotGunAmmoCount--;
        _GamePanel.UpdateShortGunAmmoText(ammoInfo.currentShotGunAmmoCount, ammoInfo.maxShotGunAmmoCount);
    }
}
