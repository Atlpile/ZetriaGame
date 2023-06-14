using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager
{
    //手枪子弹参数
    private int _packagePistolAmmo;
    private int _currentPistolAmmoLimit;
    private int _currentPistolAmmoCount;
    private int _maxPistolAmmoCount;

    //霰弹枪子弹参数
    private int _packageShotGunAmmo;
    private int _currentShotGunAmmoLimit;
    private int _currentShotGunAmmoCount;
    private int _maxShotGunAmmoCount;

    private GamePanel _GamePanel => GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>();


    public AmmoManager()
    {
        _packagePistolAmmo = 16;
        _currentPistolAmmoLimit = 8;
        _currentPistolAmmoCount = 0;
        _maxPistolAmmoCount = 0;

        _packageShotGunAmmo = 8;
        _currentShotGunAmmoLimit = 4;
        _currentShotGunAmmoCount = 0;
        _maxShotGunAmmoCount = 0;

    }


    public void PickUpPistolAmmoPackage()
    {
        _maxPistolAmmoCount += _packagePistolAmmo;
        _GamePanel.UpdatePistolAmmoText(_currentPistolAmmoCount, _maxPistolAmmoCount);
    }

    public void PickUpShotGunAmmoPackage()
    {
        _maxShotGunAmmoCount += _packageShotGunAmmo;
        _GamePanel.UpdateShortGunAmmoText(_currentShotGunAmmoCount, _maxShotGunAmmoCount);
    }


    public bool CanReload(E_PlayerStatus status)
    {
        switch (status)
        {
            case E_PlayerStatus.Pistol:
                if (_maxPistolAmmoCount > 0 && _currentPistolAmmoCount != _currentPistolAmmoLimit)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (_maxShotGunAmmoCount > 0 && _currentShotGunAmmoCount != _currentShotGunAmmoLimit)
                    return true;
                break;
        }
        return false;
    }

    public bool CanAttack(E_PlayerStatus _status)
    {
        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                if (_currentPistolAmmoCount != 0)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (_currentShotGunAmmoCount != 0)
                    return true;
                break;
        }

        return false;
    }


    public void ReloadPistolAmmo()
    {
        for (int i = _currentPistolAmmoCount; i < _currentPistolAmmoLimit; i++)
        {
            if (_maxPistolAmmoCount == 0) break;
            _maxPistolAmmoCount--;
            _currentPistolAmmoCount++;
        }
        _GamePanel.UpdatePistolAmmoText(_currentPistolAmmoCount, _maxPistolAmmoCount);
    }

    public void ReloadShotGunAmmo()
    {
        for (int i = _currentShotGunAmmoCount; i < _currentShotGunAmmoLimit; i++)
        {
            if (_maxShotGunAmmoCount == 0) break;
            _maxShotGunAmmoCount--;
            _currentShotGunAmmoCount++;
        }
        _GamePanel.UpdateShortGunAmmoText(_currentShotGunAmmoCount, _maxShotGunAmmoCount);
    }


    public void ClearAllAmmo()
    {
        _currentPistolAmmoCount = 0;
        _currentShotGunAmmoCount = 0;
        _GamePanel.UpdatePistolAmmoText(_currentPistolAmmoCount, _maxPistolAmmoCount);
        _GamePanel.UpdateShortGunAmmoText(_currentShotGunAmmoCount, _maxShotGunAmmoCount);
    }


    public void UsePistolAmmo()
    {
        // _currentPistolAmmoCount = _currentPistolAmmoCount > 0 ? _currentPistolAmmoCount-- : 0;
        _currentPistolAmmoCount--;
        _GamePanel.UpdatePistolAmmoText(_currentPistolAmmoCount, _maxPistolAmmoCount);
    }

    public void UseShotGunAmmo()
    {
        // _currentShotGunAmmoCount = _currentShotGunAmmoCount > 0 ? _currentShotGunAmmoCount-- : 0;
        _currentShotGunAmmoCount--;
        _GamePanel.UpdateShortGunAmmoText(_currentShotGunAmmoCount, _maxShotGunAmmoCount);
    }
}
