using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    // private void Awake()
    // {

    // }


    // private void Start()
    // {
    //     GameManager.Instance.m_EventManager.AddEventListener<KeyCode>(E_EventType.PressKeyDown, AKey);
    // }

    // private void AKey(KeyCode keyCode)
    // {
    //     print(keyCode + "键按下");
    // }

    private void Update()
    {
        // if (Input.GetKeyDown("tab"))
        // {
        //     print("tab键按下");
        // }

        // if (Input.anyKeyDown)
        // {
        //     // print(Input.inputString + "键按下");
        // }

        // GetKeyDownCode();

        if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.Jump))
        {
            print("跳跃键触发");
        }
        if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.SwitchWeapon))
        {
            print("切换武器键触发");
        }

        if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.Crouch))
        {
            print("下蹲按下");
        }
        if (GameManager.Instance.m_InputController.GetKey(E_InputType.Crouch))
        {
            print("长按下蹲");
        }

        if (GameManager.Instance.m_InputController.GetKeyUp(E_InputType.Crouch))
        {
            print("下蹲抬起");
        }

    }


    public KeyCode GetKeyDownCode()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Debug.Log(keyCode);
                    return keyCode;
                }
            }
        }
        return KeyCode.None;
    }


}
