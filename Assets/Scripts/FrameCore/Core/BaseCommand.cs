using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCommand : ICommand
{
    private IGameModule _gameModule;
    public IGameModule GameModule
    {
        get => _gameModule;
        set { _gameModule = value; }
    }

    void ICommand.Execute()
    {
        OnExecute();
    }

    protected abstract void OnExecute();
}
