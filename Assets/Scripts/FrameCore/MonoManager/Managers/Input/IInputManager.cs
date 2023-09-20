using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface IInputManager : IManager
    {
        bool CanInput { get; set; }
        void ChangeKey(E_InputType inputType);
        bool GetKey(E_InputType type);
        bool GetKeyDown(E_InputType type);
        bool GetKeyUp(E_InputType type);
        bool GetMouseButton(int button);
        bool GetMouseButtonDown(int button);
        bool GetMouseButtonUp(int button);
        float GetAxis(string axisName);
        float GetAxisRaw(string axisName);
    }
}


