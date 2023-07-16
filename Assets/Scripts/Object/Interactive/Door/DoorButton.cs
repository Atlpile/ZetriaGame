using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private Door _door;
    private List<GameObject> _SignLights;
    private GameObject _highLight;
    private bool _canOpen;
    private bool _isOpen;

    private void Awake()
    {
        _highLight = this.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (GameManager.Instance.InputController.GetKeyDown(E_InputType.Interacitve) && _canOpen == true)
        {
            if (_door != null && _door.type == E_DoorType.Condition)
            {
                _isOpen = !_isOpen;
                UpdateSignLights();
                _door.UpdateDoor(_isOpen);
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "door_confirm");
            }
            else if (_door != null && _door.type != E_DoorType.Condition)
            {
                Debug.Log("Door状态不为Condition,按钮无效");
            }
            else
            {
                Debug.LogWarning("没有关联指定的Door,请检查按钮是否关联对应的Door");
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

    private void UpdateSignLights()
    {
        if (_SignLights.Count == 0)
            Debug.LogWarning("未关联任何警示灯，请检查是否关联开门相关的警示灯");

        if (_isOpen)
        {
            foreach (var item in _SignLights)
            {
                item.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var item in _SignLights)
            {
                item.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void GetDoor(Door door)
    {
        _door = door;
    }

    public void GetSignLights(List<GameObject> signLights)
    {
        _SignLights = signLights;
    }
}
