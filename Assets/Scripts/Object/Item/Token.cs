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
            GameManager.Instance.m_ItemManager.GetToken(this);
            GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "collect");
            Destroy(this.gameObject);
        }
    }
}
