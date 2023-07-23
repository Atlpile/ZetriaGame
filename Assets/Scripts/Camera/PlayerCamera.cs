using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _player;
    private Vector3 _targetPos;            //Player + 摄像机 偏移的位置

    [Header("偏移设置")]
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY = 1f;

    [Header("平滑设置")]
    [SerializeField] private float _smoothTime = 0.15f;
    private Vector3 _maxSpeed = Vector3.zero;

    [Header("鼠标控制设置")]
    [SerializeField] private Vector3 _bounds = new Vector2(10, 5);
    [SerializeField] private float _mouseMoveSpeed = 25f;

    private Vector3 _screenMousePos;
    private Vector3 _limitPos;
    private float _limitX;
    private float _limitY;




    private void Awake()
    {
        _mainCamera = this.GetComponent<Camera>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            _targetPos = new Vector3(_player.position.x + _offsetX, _player.position.y + _offsetY, _player.position.z - 10f);

            //若按下鼠标右键，则摄像机跟随鼠标，否则摄像机跟随Player
            if (Input.GetMouseButton(1))
                MoveToMousePosition();
            else
                SmoothFollowPlayer();
        }
    }

    private void SmoothFollowPlayer()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, _targetPos, ref _maxSpeed, _smoothTime);
    }

    private void MoveToMousePosition()
    {
        //获取鼠标在世界位置
        _screenMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //设置摄像机范围
        _limitX = Mathf.Clamp(_screenMousePos.x, _targetPos.x - _bounds.x / 2, _targetPos.x + _bounds.x / 2);
        _limitY = Mathf.Clamp(_screenMousePos.y, _targetPos.y - _bounds.y / 2, _targetPos.y + _bounds.y / 2);
        _limitPos = new Vector3(_limitX, _limitY, -10);

        this.transform.position = Vector3.MoveTowards(this.transform.position, _limitPos, _mouseMoveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_targetPos, _bounds);
        Gizmos.DrawWireSphere(_screenMousePos, 0.5f);
    }
}


