using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public abstract class BaseGameStructure<T> : IGameStructure where T : BaseGameStructure<T>, new()
    {
        private static T s_intance;
        public static IGameStructure Instance
        {
            get
            {
                if (s_intance == null)
                {
                    // Debug.Log("Instance为空, 进行初始化");
                    s_intance = new T();
                    s_intance.RegisterModule();
                    InitModule();
                }
                return s_intance;
            }
        }

        private bool _isInited = false;
        private ModelContainer _modelContainer;
        private SystemContainer _systemContainer;
        private UtilityContainer _utilityContainer = new UtilityContainer();
        private EventContainer _eventContainer = new EventContainer();

        public BaseGameStructure()
        {
            _modelContainer = new ModelContainer(this);
            _systemContainer = new SystemContainer(this);
        }

        private static void InitModule()
        {
            s_intance._modelContainer.Init();
            s_intance._systemContainer.Init();
            s_intance._eventContainer.Init();
            s_intance._isInited = true;
        }

        protected abstract void RegisterModule();

        public void AddModel<TModel>(TModel model) where TModel : IModel
        {
            _modelContainer.AddModel<TModel>(model);
        }

        public void AddSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            _systemContainer.AddSystem<TSystem>(system);
        }

        public void AddUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            _utilityContainer.AddUtility<TUtility>(utility);
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return _modelContainer.GetModel<TModel>();
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return _systemContainer.GetSystem<TSystem>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return _utilityContainer.GetUtility<TUtility>();
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            ExecuteCommand(new TCommand());
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.GameStructure = this;
            command.Execute();
        }

        public void AddGameEvent<TStruct>(Action<TStruct> EventMethod) where TStruct : struct
        {
            _eventContainer.AddEventListener<TStruct>(EventMethod as Action<TStruct>);
        }

        public void RemoveGameEvent<TStruct>(Action<TStruct> EventMethod) where TStruct : struct
        {
            _eventContainer.RemoveReventListener<TStruct>(EventMethod as Action<TStruct>);
        }

        public void TriggerGameEvent<TStruct>(TStruct structInfo) where TStruct : struct
        {
            _eventContainer.EventTrigger<TStruct>(structInfo);
        }

    }
}
