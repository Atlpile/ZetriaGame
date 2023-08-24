using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModuleContainer
{
    void AddModule<T>(T module);
    T GetModule<T>() where T : class;
    void RemoveModule<T>();
    void ClearModule();
}
