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

    [Header("边界设置")]
    // [SerializeField] private float _boundsX;
    // [SerializeField] private float _boundsY;
    [SerializeField] private Vector3 bounds = new Vector2(10, 5);

    //鼠标
    private float _distanceToPlayer;
    private Vector3 _screenMousePos;
    [SerializeField] private float _moveSpeed = 25f;

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

            //若按下鼠标右键，则摄像机跟随鼠标
            if (Input.GetMouseButton(1))
                MoveToMousePosition();
            //否则摄像机跟随Player
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

        _screenMousePos.x = Mathf.Clamp(_screenMousePos.x, _targetPos.x - bounds.x, _targetPos.x + bounds.x);
        _screenMousePos.y = Mathf.Clamp(_screenMousePos.y, _targetPos.y - bounds.y, _targetPos.y + bounds.y);

        this.transform.position = Vector3.Lerp(_targetPos, _screenMousePos, _moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_targetPos, bounds);
    }
}


