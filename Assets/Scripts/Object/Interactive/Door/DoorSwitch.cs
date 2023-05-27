using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    private Transform _highLight;
    private Transform _signLight;
    private bool _canOpen;
    private bool _isOpen;
    public Door door;

    private void Awake()
    {
        _highLight = this.transform.GetChild(0);
        _signLight = this.transform.GetChild(1).GetChild(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canOpen == true)
        {
            if (door != null && door.type == E_DoorType.Condition)
            {
                _isOpen = !_isOpen;
                _signLight.gameObject.SetActive(_isOpen);
                door.ChangeDoor(_isOpen);
                GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "door_confirm");
            }
            else if (door != null && door.type != E_DoorType.Condition)
            {
                Debug.Log("Door状态不为Condition,按钮无效");
            }
            else
            {
                Debug.LogWarning("没有关联指定的Door脚本,请检查按钮是否关联对应的Door脚本");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _canOpen = true;
            _highLight.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _canOpen = false;
        _highLight.gameObject.SetActive(false);
    }
}
