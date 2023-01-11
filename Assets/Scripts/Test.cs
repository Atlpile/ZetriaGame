using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        // GameManager.Instance.m_SceneLoader.LoadScene("TestScene", () => { });

        // GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.UI, "MainPanel");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.UIManager.HidePanel<MainPanel>();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.UIManager.ShowPanel<MainPanel>();
            // GameManager.Instance.UIManager.ShowPanelAsync<MainPanel>((MainPanel) => { });
        }
    }
}
