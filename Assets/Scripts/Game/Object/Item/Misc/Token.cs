using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public int tokenID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.Instance.EventManager.EventTrigger<Token>(E_EventType.PickUpToken, this);
            GameManager.Instance.ItemManager.GetToken(tokenID);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "collect");
            Destroy(this.gameObject);
        }
    }
}
