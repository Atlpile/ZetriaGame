using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCardMachine : MonoBehaviour
{
    private Door _door;
    private List<GameObject> _SignLights;
    private GameObject _highLightGreen, _highLightRed;
    private bool _canUse = true;
    private bool _isPlayer;
    private bool _hasDoorCard;


    private void Awake()
    {
        _highLightGreen = this.transform.GetChild(0).gameObject;
        _highLightRed = this.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (GameManager.Instance.InputController.GetKeyDown(E_InputType.Interacitve) && _isPlayer && _canUse)
        {
            if (_hasDoorCard)
            {
                if (_door != null && _door.type == E_DoorType.Condition)
                {
                    _canUse = false;
                    _door.UpdateDoor(true);
                    UpdateSignLights();
                    GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "door_confirm");
                }
            }
            else
            {
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "door_error");
                ControllerError();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLightGreen.SetActive(true);
            _isPlayer = true;

            if (other.GetComponent<PlayerController>().ZetriaInfo.hasDoorCard)
                _hasDoorCard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLightGreen.SetActive(false);
            _isPlayer = false;
            _hasDoorCard = false;
        }
    }

    private void UpdateSignLights()
    {
        if (_SignLights.Count == 0)
            Debug.LogWarning("未关联任何警示灯，请检查是否关联开门相关的警示灯");


        foreach (var item in _SignLights)
        {
            item.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void ControllerError()
    {
        StartCoroutine(IE_ControllerError());
    }

    private IEnumerator IE_ControllerError()
    {
        _highLightRed.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _highLightRed.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _highLightRed.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _highLightRed.SetActive(false);
    }

    public void GetDoor(Door door)
    {
        _door = door;
    }

    public void GetSignLights(List<GameObject> SignLights)
    {
        _SignLights = SignLights;
    }
}
