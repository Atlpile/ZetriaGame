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
    private bool canInput;

    public Dictionary<E_InputType, KeyCode> DefaultInputDic;
    public Dictionary<E_InputType, KeyCode> CustomInputDic;

    public InputController()
    {
        canInput = true;

        //默认按键配置
        DefaultInputDic = new Dictionary<E_InputType, KeyCode>
        {
            {E_InputType.SwitchWeapon,  KeyCode.Tab},
            {E_InputType.Crouch,        KeyCode.S},
            {E_InputType.GunAttack,     KeyCode.K},
            {E_InputType.Interacitve,   KeyCode.E},
            {E_InputType.Jump,          KeyCode.Space},
            {E_InputType.MeleeAttack,   KeyCode.J},
            {E_InputType.Pause,         KeyCode.Escape},
            {E_InputType.Reload,        KeyCode.R},
            {E_InputType.PutDownNPC,    KeyCode.F},
        };
        CustomInputDic = DefaultInputDic;
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
        KeyCode key = GetKeyDownCode();
        if (key != KeyCode.None)
            CustomInputDic[inputType] = key;
        else
            Debug.Log("Key为空,没有该按键");
    }


    public bool GetKey(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic.ContainsKey(type) && Input.GetKey(CustomInputDic[type]))
                return Input.GetKey(CustomInputDic[type]);
        }
        return false;

    }

    public bool GetKeyDown(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic.ContainsKey(type) && Input.GetKeyDown(CustomInputDic[type]))
                return Input.GetKeyDown(CustomInputDic[type]);
        }
        return false;
    }

    public bool GetKeyUp(E_InputType type)
    {
        if (canInput)
        {
            if (CustomInputDic.ContainsKey(type) && Input.GetKeyUp(CustomInputDic[type]))
                return Input.GetKeyUp(CustomInputDic[type]);
        }
        return false;
    }

    public bool GetMouseButton(int button)
    {
        if (canInput)
        {
            return Input.GetMouseButton(button);
        }

        return false;
    }

    public bool GetMouseButtonDown(int button)
    {
        if (canInput)
        {
            return Input.GetMouseButtonDown(button);
        }

        return false;
    }

    public bool GetMouseButtonUp(int button)
    {
        if (canInput)
        {
            return Input.GetMouseButtonUp(button);
        }

        return false;
    }

    public float GetAxis(string axisName)
    {
        if (canInput)
        {
            return Input.GetAxis(axisName);
        }
        return 0;
    }

    public float GetAxisRaw(string axisName)
    {
        if (canInput)
        {
            return Input.GetAxisRaw(axisName);
        }
        return 0;
    }

    public void SetInputStatus(bool canInput)
    {
        this.canInput = canInput;
    }

}



