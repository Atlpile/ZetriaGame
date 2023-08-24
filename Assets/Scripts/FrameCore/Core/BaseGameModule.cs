using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameModule<T> : IGameModule where T : BaseGameModule<T>, new()
{
    private static T _intanceModule;
    public static IGameModule InstanceModule
    {
        get
        {
            if (_intanceModule == null)
            {
                Debug.Log("Instance为空，进行初始化");
                InitModule();
            }
            Debug.Log("Instance不为空");
            return _intanceModule;
        }
    }

    private bool _isInited = false;
    private ModuleContainer _moduleContainer = new ModuleContainer();
    private readonly HashSet<IModel> _Models = new HashSet<IModel>();
    private readonly HashSet<ISystem> _Systems = new HashSet<ISystem>();
    private readonly HashSet<IUtility> _Utility = new HashSet<IUtility>();


    private static void InitModule()
    {
        _intanceModule = new T();
        _intanceModule.RegisterModule();

        foreach (var model in _intanceModule._Models)
        {
            model.Init();
        }
        // _intanceModule._Models.Clear();

        foreach (var system in _intanceModule._Systems)
        {
            system.Init();
        }
        // _intanceModule._Systems.Clear();

        _intanceModule._isInited = true;
    }

    protected abstract void RegisterModule();


    public void AddSystem<TSystem>(TSystem system) where TSystem : ISystem
    {
        system.GameModule = this;
        _moduleContainer.AddModule<TSystem>(system);

        if (_isInited)
            system.Init();
        else
            _Systems.Add(system);
    }

    public void AddModel<TModel>(TModel model) where TModel : IModel
    {
        model.GameModule = this;
        _moduleContainer.AddModule<TModel>(model);

        if (_isInited)
            model.Init();
        else
            _Models.Add(model);
    }

    public void AddUtility<TUtility>(TUtility utility) where TUtility : IUtility
    {
        _moduleContainer.AddModule<TUtility>(utility);
    }

    public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
    {
        return _moduleContainer.GetModule<TSystem>();
    }

    public TModel GetModel<TModel>() where TModel : class, IModel
    {
        return _moduleContainer.GetModule<TModel>();
    }

    public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
    {
        return _moduleContainer.GetModule<TUtility>();
    }


    public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
    {
        ExecuteCommand(command);
    }

    protected virtual void ExecuteCommand(ICommand command)
    {
        command.GameModule = this;
        command.Execute();
    }

}
