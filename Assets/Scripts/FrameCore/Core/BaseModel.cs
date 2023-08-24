using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModel : IModel
{
    private IGameModule _gameModule;

    public IGameModule GameModule
    {
        get => _gameModule;
        set { _gameModule = value; }
    }

    public void Init()
    {
        OnInit();
    }

    protected abstract void OnInit();
}
