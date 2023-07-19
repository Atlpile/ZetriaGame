using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    private GameObject _highLight;
    private GameObject _box;
    private Transform _spawnPos;
    private bool _canOpen;
    private bool _isCreate;

    private void Awake()
    {
        _highLight = this.transform.GetChild(0).gameObject;
        _box = this.transform.GetChild(1).gameObject;
        _spawnPos = this.transform.GetChild(2);
    }

    private void Update()
    {
        if (GameManager.Instance.InputController.GetKeyDown(E_InputType.Interacitve) && _canOpen == true)
        {
            SpawnBox();
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

    private void SpawnBox()
    {
        if (_isCreate == false)
        {
            _box.gameObject.SetActive(true);
            _isCreate = true;
        }


        if (_isCreate == true)
        {
            //在未回归前生成Box消失效果
            //Box回归创建位置
            _box.transform.position = _spawnPos.position;
            _box.transform.rotation = _spawnPos.rotation;
            _box.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //创建Spawn生成效果
            //播放创建音效
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "spawn");
        }

    }
}
