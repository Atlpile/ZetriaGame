using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetPoint;

    private GameObject _highLight;
    private bool _isPlayer;


    private void Awake()
    {
        _highLight = this.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        if (targetPoint == null)
        {
            Debug.Log("传送点为空，请检查传送点是否挂载");
        }
    }

    private void Update()
    {
        if (targetPoint != null)
        {
            if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.Interacitve) && _isPlayer)
            {
                TeleportToTarget();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _isPlayer = true;
            _highLight.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _isPlayer = false;
            _highLight.gameObject.SetActive(false);
        }
    }

    private void TeleportToTarget()
    {
        GameManager.Instance.m_EventManager.EventTrigger<Vector3>(E_EventType.PlayerTeleport, targetPoint.transform.position);
        GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "teleport_player");
        GameObject fx = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("FX_Teleport", E_ResourcesPath.FX);
        fx.transform.position = targetPoint.transform.position;
        //TODO:生成粒子效果
    }
}
