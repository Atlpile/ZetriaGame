using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.m_EventManager.EventTrigger<Token>(E_EventType.PickUpToken, this);
            //TODO:拾取音效
            Destroy(this.gameObject);
        }
    }
}
