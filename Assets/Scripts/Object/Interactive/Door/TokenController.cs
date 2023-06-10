using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour
{
    public int needTokenNum;
    public Door door;
    public GameObject[] signLights;

    private GameObject _highLightGreen;
    private GameObject _highLightRed;
    private bool _isPlayer;
    private bool _canUse = true;

    private void Awake()
    {
        _highLightGreen = this.transform.GetChild(0).gameObject;
        _highLightRed = this.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (GameManager.Instance.m_InputController.GetKeyDown(E_InputType.Interacitve) && _isPlayer && _canUse)
        {
            GameData gameData = GameManager.Instance.m_SaveLoadManager.LoadData<GameData>("GameData");
            if (gameData.TokenDic.Count >= needTokenNum)
            {
                if (door != null && door.type == E_DoorType.Condition)
                {
                    Debug.Log("令牌数量足够");
                    _canUse = false;
                    door.UpdateDoor(true);
                    GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "door_confirm");
                }
            }
            else
            {
                Debug.Log("令牌数量不够");
                GameManager.Instance.m_AudioController.AudioPlay(E_AudioType.Effect, "door_error");
                ControllerError();
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLightGreen.gameObject.SetActive(true);
            _isPlayer = true;


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLightGreen.gameObject.SetActive(false);
            _isPlayer = false;
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

}
