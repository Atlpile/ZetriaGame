using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInfo
{
    //手枪子弹参数
    public int _packagePistolAmmoCount;
    public int _currentPistolAmmoLimit;
    public int _currentPistolAmmoCount;
    public int _maxPistolAmmoCount;

    //霰弹枪子弹参数
    public int _packageShotGunAmmoCount;
    public int _currentShotGunAmmoLimit;
    public int _currentShotGunAmmoCount;
    public int _maxShotGunAmmoCount;

    public AmmoInfo()
    {
        _packagePistolAmmoCount = 16;
        _currentPistolAmmoLimit = 8;
        _currentPistolAmmoCount = 0;
        _maxPistolAmmoCount = 0;

        _packageShotGunAmmoCount = 8;
        _currentShotGunAmmoLimit = 4;
        _currentShotGunAmmoCount = 0;
        _maxShotGunAmmoCount = 0;
    }
}
