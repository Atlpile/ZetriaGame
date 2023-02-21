using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    private void Update()
    {
        AudioTest();
    }

    private void ObjTest()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // GameObject obj = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("PistolBullet", E_ResourcesPath.Entity);
            // GameObject obj = GameManager.Instance.m_ObjectPool.GetOrLoadObject("PistolBullet", E_ResourcesPath.Entity);
            // obj.transform.position = this.transform.position;

            GameManager.Instance.m_ObjectPoolManager.AddObjectFromResources("PistolBullet", E_ResourcesPath.Entity);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject obj = GameManager.Instance.m_ObjectPoolManager.GetObject("PistolBullet");
            obj.transform.position = this.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.m_ObjectPoolManager.ClearPool();
        }
    }

    private void AudioTest()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "pistol_fire");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.BGM, "bgm_1", true);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            GameManager.Instance.m_AudioController.BGMPause();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.m_AudioController.BGMResume();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.m_AudioController.BGMStop();
        }
    }
}
