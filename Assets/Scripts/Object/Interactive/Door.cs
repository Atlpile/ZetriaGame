using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseEntity
{
    public E_DoorType type;

    protected override void OnAwake()
    {
        anim = this.GetComponentInChildren<Animator>();
    }

    protected override void OnStart()
    {
        switch (type)
        {
            case E_DoorType.Once:
                anim.SetBool("IsOpen", true);
                break;
            case E_DoorType.Condition:
            case E_DoorType.Smart:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == E_DoorType.Smart)
        {
            anim.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            anim.SetBool("IsOpen", false);
        }
    }
}
