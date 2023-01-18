using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private bool canInput;


    public InputManager()
    {

    }

    public void SetInputStatus(bool inputStatus)
    {
        canInput = inputStatus;
    }

    public void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
            GameManager.Instance.m_EventManager.EventTrigger("T键按下");

        if (Input.GetKey(KeyCode.Y))
            GameManager.Instance.m_EventManager.EventTrigger("Y键长按");
    }


}



