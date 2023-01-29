using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseEntity
{
    //Smart：自动开，自动关
    //Condition：条件开，条件关
    //Once：只关一次
    public E_DoorType type;
    private BoxCollider2D boxColl2D;

    protected override void OnAwake()
    {
        anim = this.GetComponentInChildren<Animator>();
        boxColl2D = this.GetComponent<BoxCollider2D>();
    }

    protected override void OnStart()
    {
        if (type == E_DoorType.Once)
        {
            anim.SetBool("IsOpen", true);
            boxColl2D.offset = new Vector2(-1, 0);
            boxColl2D.size = new Vector2(1, 2);
        }


        else if (type == E_DoorType.Condition)
            boxColl2D.enabled = false;
    }

    //FIXME:Once时重新设置碰撞体范围

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == E_DoorType.Smart)
        {
            ChangeDoor(true);
        }
        else if (other.gameObject.name == "Player" && type == E_DoorType.Once)
        {
            ChangeDoor(false);
            boxColl2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == E_DoorType.Smart)
        {
            ChangeDoor(false);
        }
    }



    public void ChangeDoor(bool isOpen)
    {
        anim.SetBool("IsOpen", isOpen);
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "door_shut");
    }
}
