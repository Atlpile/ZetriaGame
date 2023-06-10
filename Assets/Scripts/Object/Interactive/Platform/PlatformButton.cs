using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    按钮功能
        1.按下按钮，平台向目标点移动，播放移动音效
        2.到达目标点后，按钮熄灭，播放到达音效
        3.未到达目标点，按钮亮起
*/


public class PlatformButton : MonoBehaviour
{
    public Transform targetPoint;

    private PlatformController _platformController;
    private GameObject _buttonLight;
    private GameObject _highLight;
    private bool _canUse;
    private bool _isInteractive;


    private void Awake()
    {
        _buttonLight = this.transform.GetChild(0).gameObject;
        _highLight = this.transform.GetChild(1).gameObject;

        _platformController = this.GetComponentInParent<PlatformController>();
    }

    private void Start()
    {
        //检查是否挂载需要的物体
        if (targetPoint == null)
            Debug.LogWarning(this.gameObject.name + "没有挂载目标点,请检查该物体目标点是否挂载");

        //初始化按钮灯的状态
        UpdateButtonLight();
    }

    private void Update()
    {
        //更新按钮输入
        _isInteractive = GameManager.Instance.m_InputController.GetKey(E_InputType.Interacitve) ? true : false;
    }

    private void FixedUpdate()
    {
        //更新平台移动
        if (targetPoint != null)
        {
            UpdateButtonLight();

            if (_isInteractive && _canUse)
            {
                if (IsArriveTarget())
                {
                    _platformController.PlatformStop();
                }
                else
                {
                    _platformController.PlatformMove(targetPoint);
                }
            }
            else if (!_isInteractive && _canUse)
            {
                _platformController.PlatformStop();
            }
        }
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


    public void UpdateButtonLight()
    {
        if (IsArriveTarget())
            _buttonLight.gameObject.SetActive(false);
        else
            _buttonLight.gameObject.SetActive(true);
    }

    private bool IsArriveTarget()
    {
        if (Vector2.Distance(_platformController.platform.transform.position, targetPoint.position) <= 0.1f)
            return true;
        else
            return false;
    }





}
