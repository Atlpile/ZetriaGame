using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Start()
    {
        // GameManager.Instance.m_EventManager.AddEventListener("T键按下", PickUp);
        // GameManager.Instance.m_EventManager.AddEventListener("Y键长按", PressKey);
    }

    // private void OnEnable()
    // {
    //     GameManager.Instance.m_EventManager.AddEventListener<KeyCode>("某键按下", PickUp);
    // }

    // private void OnDisable()
    // {
    //     GameManager.Instance.m_EventManager.RemoveEventListener<KeyCode>("某键按下", PickUp);
    // }

    private void PickUp()
    {
        Debug.Log("T键按下");
    }

    private void PressKey()
    {
        Debug.Log("Y键长按");
    }
}
