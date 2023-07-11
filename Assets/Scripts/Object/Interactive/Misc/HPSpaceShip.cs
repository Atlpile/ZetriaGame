using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSpaceShip : MonoBehaviour
{
    public int hp;

    private GameObject _keyTip;
    private bool _canUse = true;
    private bool _isPlayer;


    private void Awake()
    {
        _keyTip = this.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (GameManager.Instance.InputController.GetKeyDown(E_InputType.Interacitve) && _isPlayer && _canUse)
        {
            AddHP();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _keyTip.gameObject.SetActive(true);
            _isPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _keyTip.gameObject.SetActive(false);
            _isPlayer = false;
        }
    }

    private void AddHP()
    {
        // GameManager.Instance.m_EventManager.EventTrigger<int>(E_EventType.PlayerAddHP, hp);
        Debug.Log("恢复" + hp + "血量");
        _canUse = false;
        //播放加血音效
    }
}
