using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInfo
{
    //手枪子弹参数
    public int packagePistolAmmoCount;
    public int currentPistolAmmoLimit;
    public int currentPistolAmmoCount;
    public int maxPistolAmmoCount;

    //霰弹枪子弹参数
    public int packageShotGunAmmoCount;
    public int currentShotGunAmmoLimit;
    public int currentShotGunAmmoCount;
    public int maxShotGunAmmoCount;

    public AmmoInfo()
    {
        packagePistolAmmoCount = 16;
        currentPistolAmmoLimit = 8;
        currentPistolAmmoCount = 0;
        maxPistolAmmoCount = 0;

        packageShotGunAmmoCount = 8;
        currentShotGunAmmoLimit = 4;
        currentShotGunAmmoCount = 0;
        maxShotGunAmmoCount = 0;
    }
}
