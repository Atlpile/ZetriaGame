using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public sealed class InputManager : BaseManager, IInputManager
    {
        public bool CanInput { get; set; }
        private Dictionary<E_InputTypes, KeyCode> _DefaultInputKeyContainer;
        private Dictionary<E_InputTypes, KeyCode> _CustomInputKeyContainer;


        public InputManager(MonoManager manager) : base(manager) { }

        protected override void OnInit()
        {
            CanInput = true;
            //默认按键配置
            _DefaultInputKeyContainer = new Config_Input().DefaultConfig;
            //自定义按键配置
            _CustomInputKeyContainer = _DefaultInputKeyContainer;
        }


        private KeyCode GetAnyKeyDown()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        return keyCode;
                    }
                }
            }
            return KeyCode.None;
        }

        public void ChangeKey(E_InputTypes type)
        {
            KeyCode key = GetAnyKeyDown();
            if (key != KeyCode.None)
                _CustomInputKeyContainer[type] = key;
            else
                Debug.Log("Key为空,没有该按键");
        }

        public bool GetKey(E_InputTypes type)
        {
            if (CanInput)
            {
                if (_CustomInputKeyContainer.ContainsKey(type) && Input.GetKey(_CustomInputKeyContainer[type]))
                    return Input.GetKey(_CustomInputKeyContainer[type]);
            }
            return false;
        }

        public bool GetKeyDown(E_InputTypes type)
        {
            if (CanInput)
            {
                if (_CustomInputKeyContainer.ContainsKey(type) && Input.GetKeyDown(_CustomInputKeyContainer[type]))
                    return Input.GetKeyDown(_CustomInputKeyContainer[type]);
            }
            return false;
        }

        public bool GetKeyUp(E_InputTypes type)
        {
            if (CanInput)
            {
                if (_CustomInputKeyContainer.ContainsKey(type) && Input.GetKeyUp(_CustomInputKeyContainer[type]))
                    return Input.GetKeyUp(_CustomInputKeyContainer[type]);
            }
            return false;
        }

        public bool GetMouseButton(int button)
        {
            if (CanInput)
            {
                return Input.GetMouseButton(button);
            }

            return false;
        }

        public bool GetMouseButtonDown(int button)
        {
            if (CanInput)
            {
                return Input.GetMouseButtonDown(button);
            }

            return false;
        }

        public bool GetMouseButtonUp(int button)
        {
            if (CanInput)
            {
                return Input.GetMouseButtonUp(button);
            }

            return false;
        }

        public float GetAxis(string axisName)
        {
            if (CanInput)
            {
                return Input.GetAxis(axisName);
            }
            return 0;
        }

        public float GetAxisRaw(string axisName)
        {
            if (CanInput)
            {
                return Input.GetAxisRaw(axisName);
            }
            return 0;
        }

    }
}


