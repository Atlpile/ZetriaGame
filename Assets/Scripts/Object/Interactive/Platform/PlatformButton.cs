using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public Transform targetPoint;
    private GameObject _closeLight;
    private GameObject _highLight;
    public bool isMax;

    private PlatformController _controller;
    private bool _canUse;


    private void Awake()
    {
        _closeLight = this.transform.GetChild(0).gameObject;
        _highLight = this.transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        if (targetPoint == null)
            Debug.LogWarning(this.gameObject.name + "没有挂载目标点,请检查该物体目标点是否挂载");
    }

    // private void Update()
    // {
    //     // if (Input.GetKeyDown(KeyCode.E) && !isMax)
    //     // {
    //     //     _controller.platformAudioSource.enabled = true;
    //     // }
    //     // else if (Input.GetKeyUp(KeyCode.E))
    //     // {
    //     //     _controller.platformAudioSource.enabled = false;
    //     // }
    //     // else if (isMax)
    //     // {
    //     //     _controller.platformAudioSource.enabled = false;
    //     // }

    // }

    private void FixedUpdate()
    {
        if (targetPoint != null && _controller != null)
        {
            // isMax = Vector2.Distance(_controller.platform.transform.position, targetPoint.position) < 0.1f;
            // UpdateButtonLight();

            // if (Input.GetKey(KeyCode.E) && _canUse && !isMax)
            // {
            //     _controller.PlatformMove(targetPoint);
            // }


            if (Input.GetKeyDown(KeyCode.E) && _canUse && isMax)
            {
                _controller.InitAllButtons(this);
                isMax = false;
            }
            if (Input.GetKey(KeyCode.E) && _canUse && !isMax)
            {
                //移动平台
                _controller.PlatformMove(this);

                //开启音效
                if (_controller.platformAudioSource.enabled == false)
                    _controller.platformAudioSource.enabled = true;
            }
            if (Input.GetKeyUp(KeyCode.E) && _canUse && !isMax)
            {
                //关闭音效
                _controller.platformAudioSource.enabled = false;
            }

        }
    }



    public void UpdateButtonLight()
    {
        if (isMax && !_closeLight.activeInHierarchy)
            _closeLight.gameObject.SetActive(true);
        else if (!isMax && _closeLight.activeInHierarchy)
            _closeLight.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(true);
            _canUse = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            _highLight.SetActive(false);
            _canUse = false;
        }
    }

    public void OpenButtonLight()
    {
        _closeLight.SetActive(true);
    }

    public void ButtonMax()
    {
        isMax = true;
    }





    public void RegisterPlatformController(PlatformController controller)
    {
        this._controller = controller;
    }
}
