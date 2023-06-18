using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolPanelTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.m_UIManager.ShowPanelFromPool<MainPanel>();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.m_UIManager.HidePanelFromPool<MainPanel>();
        }
    }
}
