using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Keypad4))
        // {
        //     GameManager.Instance.ObjectPoolManager.RemovePoolStack("ShortGunBullet", "AudioPlayer");
        // }
        // else if (Input.GetKeyDown(KeyCode.Keypad5))
        // {
        //     GameManager.Instance.ObjectPoolManager.RemoveExcept("ShortGunBullet");
        // }
        // else if (Input.GetKeyDown(KeyCode.Keypad6))
        // {
        //     GameManager.Instance.ObjectPoolManager.RemoveExcept("ShortGunBullet", "PistolBullet", "AudioPlayer");
        // }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            GameManager.Instance.UIManager.ShowPanel_Pool_Async<FadePanel>(true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            GameManager.Instance.UIManager.HidePanel_Pool<FadePanel>(true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {

        }
    }
}
