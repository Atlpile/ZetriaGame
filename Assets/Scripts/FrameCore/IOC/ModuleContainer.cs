using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleContainer : IModuleContainer
{
    private Dictionary<Type, object> _ModuleContainer = new Dictionary<Type, object>();

    public void AddModule<T>(T module)
    {
        Type moduleType = typeof(T);
        if (_ModuleContainer.ContainsKey(moduleType))
        {
            //防止Model重复添加
            _ModuleContainer[moduleType] = module;
        }
        else
        {
            //没有Model则新添加Model
            _ModuleContainer.Add(moduleType, module);
        }
    }

    public T GetModule<T>() where T : class
    {
        Type moduleType = typeof(T);
        if (_ModuleContainer.TryGetValue(moduleType, out object module))
        {
            return module as T;
        }
        Debug.LogError("模块中不存在" + moduleType.Name + "，请检查模块中是否注册该模块");
        return null;

    }

    public void RemoveModule<T>()
    {
        Type moduleType = typeof(T);
        if (_ModuleContainer.TryGetValue(moduleType, out object module))
        {
            _ModuleContainer.Remove(moduleType);
        }
        else
        {
            Debug.LogError("不存在该类型，请检查类型是否存在");
        }
    }

    public void ClearModule()
    {
        _ModuleContainer.Clear();
    }
}
