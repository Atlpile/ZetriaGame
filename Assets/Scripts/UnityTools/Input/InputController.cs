using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    输入管理器功能
        2.可以控制输入的启用和禁用
        3.可以执行改键操作
        4.可以配置按键
*/


public class InputController
{
    public bool canInput;

    // //默认输入
    // public Dictionary<E_InputType, string> DefaultInputDic;
    // //自定义输入
    // public Dictionary<E_InputType, string> CustomInputDic;

    public Dictionary<E_InputType, KeyCode> DefaultInputDic2;
    public Dictionary<E_InputType, KeyCode> CustomInputDic2;

    public Dictionary<string, bool> AxisKeyDic;


    public InputController()
    {
        canInput = true;

        // //默认输入配置
        // DefaultInputDic = new Dictionary<E_InputType, string>
        // {
        //     {E_InputType.AlterWeapon,   "Tab"   },
        //     {E_InputType.Crouch,        "S"     },
        //     {E_InputType.GunAttack,     "K"     },
        //     {E_InputType.Interacitve,   "E"     },
        //     {E_InputType.Jump,          "Space" },
        //     {E_InputType.MeleeAttack,   "J"     },
        //     {E_InputType.Pause,         "Escape"},
        //     {E_InputType.Reload,        "R"     },
        // };
        // CustomInputDic = DefaultInputDic;

        DefaultInputDic2 = new Dictionary<E_InputType, KeyCode>
        {
            {E_InputType.SwitchWeapon,  KeyCode.Tab},
            {E_InputType.Crouch,        KeyCode.S},
            {E_InputType.GunAttack,     KeyCode.K},
            {E_InputType.Interacitve,   KeyCode.E},
            {E_InputType.Jump,          KeyCode.Space},
            {E_InputType.MeleeAttack,   KeyCode.J},
            {E_InputType.Pause,         KeyCode.Escape},
            {E_InputType.Reload,        KeyCode.R},
        };
        CustomInputDic2 = DefaultInputDic2;
    }

    //获取任意按键
    private KeyCode GetKeyDownCode()
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

    public void ChangeKey(E_InputType inputType)
    {
        if (CustomInputDic2.ContainsKey(inputType))
        {
            CustomInputDic2[inputType] = GetKeyDownCode();
        }
    }


    public bool GetKey(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic2.ContainsKey(type) && Input.GetKey(CustomInputDic2[type]))
                return Input.GetKey(CustomInputDic2[type]);
        }
        return false;

    }

    public bool GetKeyDown(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic2.ContainsKey(type) && Input.GetKeyDown(CustomInputDic2[type]))
                return Input.GetKeyDown(CustomInputDic2[type]);
        }
        return false;
    }

    public bool GetKeyUp(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic2.ContainsKey(type) && Input.GetKeyUp(CustomInputDic2[type]))
                return Input.GetKeyUp(CustomInputDic2[type]);

        }
        return false;
    }


}



