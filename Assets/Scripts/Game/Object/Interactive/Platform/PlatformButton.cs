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

    private GameObject _buttonLight;
    private GameObject _highLight;
    private Platform _platform;
    private bool _canUse;
    private bool _isInteractive;


    private void Awake()
    {
        _buttonLight = this.transform.GetChild(0).gameObject;
        _highLight = this.transform.GetChild(1).gameObject;
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
        _isInteractive = GameManager.Instance.InputController.GetKey(E_InputType.Interacitve);
    }

    private void FixedUpdate()
    {
        //更新平台移动
        if (targetPoint != null)
        {
            UpdateButtonLight();

            //PLayer在触发范围内，且按下交互
            if (_canUse && _isInteractive)
            {
                //若到达目标点
                if (IsArriveTarget())
                {
                    _platform.UpdateAudioPlay(false);
                }
                else
                {
                    _platform.Move(targetPoint);
                }
            }
            //Player在触发范围内，但未按交互
            else if (_canUse && !_isInteractive)
            {
                // _platform.Move(targetPoint);
                // _platform.UpdateAudioPlay(true);
            }
        }
    }

    public void UpdateButtonLight()
    {
        if (IsArriveTarget())
            _buttonLight.SetActive(false);
        else
            _buttonLight.SetActive(true);
    }

    private bool IsArriveTarget()
    {
        return Vector2.Distance(_platform.transform.position, targetPoint.position) <= 0.1f;
    }

    public void GetPlatform(Platform platform)
    {
        _platform = platform;
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

            //TODO:可优化为event来解耦合
            //Player离开时仍进行输入，则平台直接停止
            _platform.UpdateAudioPlay(false);
        }
    }




}
