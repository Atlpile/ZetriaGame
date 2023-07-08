using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera _mainCamera;

    [Header("平滑设置")]

    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY = 1f;
    [SerializeField] private float _smoothTime = 0.15f;

    [Header("边界设置")]
    [SerializeField] private float _boundsX;
    [SerializeField] private float _boundsY;

    private Transform _playerPos;
    private Vector3 _maxSpeed = Vector3.zero;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _mainCamera = this.GetComponent<Camera>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        SmoothFollow(_playerPos);
    }


    private void SmoothFollow(Transform target)
    {
        if (target)
        {
            _targetPosition = new Vector3(target.position.x + _offsetX, target.position.y + _offsetY, target.position.z - 10f);
            this.transform.position = Vector3.SmoothDamp(this.transform.position, _targetPosition, ref _maxSpeed, _smoothTime);
        }
    }

    private void MouseMoveCamera()
    {

    }

    private bool CanMouseMove()
    {
        return false;
    }

}


