using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    public bool IsPause { get; set; }

    public void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameObject.FindGameObjectWithTag("Player"))
        {
            UpdateGameStatus();
        }
    }

    public void UpdateGameStatus()
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            // Time.timeScale = 0;
            GameManager.Instance.m_UIManager.ShowPanel<PausePanel>();
            GameManager.Instance.m_InputController.SetInputStatus(false);
        }
        else
        {
            // Time.timeScale = 1;
            GameManager.Instance.m_UIManager.HidePanel<PausePanel>();
            GameManager.Instance.m_InputController.SetInputStatus(true);
        }
    }
}
