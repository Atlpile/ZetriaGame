using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameModule
{
    void AddSystem<T>(T controller) where T : ISystem;
    void AddModel<T>(T model) where T : IModel;

    T GetSystem<T>() where T : class, ISystem;
    T GetModel<T>() where T : class, IModel;

    void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;
}
