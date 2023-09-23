using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IInputManager : IManager
    {
        bool CanInput { get; set; }
        void ChangeKey(E_InputTypes inputType);
        bool GetKey(E_InputTypes type);
        bool GetKeyDown(E_InputTypes type);
        bool GetKeyUp(E_InputTypes type);
        bool GetMouseButton(int button);
        bool GetMouseButtonDown(int button);
        bool GetMouseButtonUp(int button);
        float GetAxis(string axisName);
        float GetAxisRaw(string axisName);
    }
}


