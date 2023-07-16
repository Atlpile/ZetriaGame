using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Smart：自动开，自动关
//Condition：条件开，条件关
//Once：只关一次

public class Door : MonoBehaviour
{
    public E_DoorType type;
    private BoxCollider2D _boxColl2D;
    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _boxColl2D = this.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (type == E_DoorType.Once)
        {
            _animator.SetBool("IsOpen", true);
            _boxColl2D.offset = new Vector2(-1, 0);
            _boxColl2D.size = new Vector2(1, 2);
        }
        else if (type == E_DoorType.Condition)
        {
            _boxColl2D.enabled = false;
        }
    }

    //FIXME:Once时重新设置碰撞体范围
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == E_DoorType.Smart)
        {
            UpdateDoor(true);
        }
        else if (other.gameObject.name == "Player" && type == E_DoorType.Once)
        {
            UpdateDoor(false);
            _boxColl2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && type == E_DoorType.Smart)
        {
            UpdateDoor(false);
        }
    }

    public void UpdateDoor(bool isOpen)
    {
        _animator.SetBool("IsOpen", isOpen);
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "door_shut");
    }
}
