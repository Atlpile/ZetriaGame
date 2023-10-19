using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IGameStructure
    {
        void AddSystem<T>(T controller) where T : ISystem;
        void AddModel<T>(T model) where T : IModel;
        void AddUtility<T>(T utility) where T : IUtility;

        T GetSystem<T>() where T : class, ISystem;
        T GetModel<T>() where T : class, IModel;
        T GetUtility<T>() where T : class, IUtility;

        void SendCommand<TCommand>(TCommand command) where TCommand : ICommand;

        void AddGameEventListener<TStruct>(Action<TStruct> EventMethod) where TStruct : struct;
        void RemoveGameEventListener<TStruct>(Action<TStruct> EventMethod) where TStruct : struct;
        void TriggerGameEvent<TStruct>(TStruct structInfo) where TStruct : struct;
    }
}
