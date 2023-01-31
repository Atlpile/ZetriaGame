using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepWomen : BaseCharacter
{
    private bool _canPickUp;

    protected override void OnStart()
    {
        base.OnStart();

        rb2D.freezeRotation = true;
        rb2D.drag = 1.5f;
        rb2D.gravityScale = 1.5f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Q) && _canPickUp)
        {
            Hide();
            GameManager.Instance.m_EventManager.EventTrigger(E_EventType.PickUpNPC);
            GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "npc_pickup");
        }
    }

    private void Hide()
    {
        GameManager.Instance.m_ObjectPool.ReturnObject(this.gameObject.name, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _canPickUp = false;
        }
    }

}
