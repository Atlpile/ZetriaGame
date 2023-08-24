using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSystem : ISystem
{
    private IGameModule _gameModule;

    public IGameModule GameModule
    {
        get => _gameModule;
        set { _gameModule = value; }
    }

    void ISystem.Init()
    {
        OnInit();
    }

    protected abstract void OnInit();


}
