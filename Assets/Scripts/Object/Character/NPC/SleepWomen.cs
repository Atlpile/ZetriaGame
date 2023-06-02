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

        if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.PickUpNPC) && _canPickUp)
        {
            //OPTIMIZE：拾取与放下NPC，当按键重复时，会重叠导致第一次拾取后立即放下，存在先后逻辑
            GameManager.Instance.m_EventManager.EventTrigger(E_EventType.PickUpNPC);
            GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "npc_pickup");
            Hide();
        }
    }

    private void Hide()
    {
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(this.gameObject);
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
