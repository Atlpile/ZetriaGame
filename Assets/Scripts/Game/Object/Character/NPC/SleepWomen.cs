using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepWomen : BaseCharacter, IObject
{
    private bool _canPickUp;

    protected override void OnStart()
    {
        rb2D.freezeRotation = true;
        rb2D.drag = 1.5f;
        rb2D.gravityScale = 1.5f;

        GameManager.Instance.ObjectPoolManager.AddObject(this.gameObject);
    }

    private void OnEnable()
    {
        Create();
    }

    private void OnDisable()
    {
        Release();
    }

    protected override void OnUpdate()
    {
        if (GameManager.Instance.InputController.GetKeyDown(E_InputType.PickUpNPC) && _canPickUp)
        {
            //OPTIMIZE：拾取与放下NPC，当按键重复时，会重叠导致第一次拾取后立即放下，存在先后逻辑
            GameManager.Instance.EventManager.EventTrigger(E_EventType.PickUpNPC);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "npc_pickup");
            Hide();
        }
    }

    public void Create()
    {

    }

    public void Release()
    {

    }

    public void Hide()
    {
        GameManager.Instance.ObjectPoolManager.ReturnObject(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            _canPickUp = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            _canPickUp = false;
    }

}
