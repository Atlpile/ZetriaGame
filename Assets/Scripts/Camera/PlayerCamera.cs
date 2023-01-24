using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera mainCamera;

    [Header("平滑设置")]

    public float offsetX;
    public float offsetY = 1f;
    public float smoothTime = 0.15f;

    [Header("边界设置")]
    public float boundsX;
    public float boundsY;

    private Transform playerPos;
    private Vector3 _maxSpeed = Vector3.zero;
    private Vector3 targetPosition;

    private void Awake()
    {
        mainCamera = this.GetComponent<Camera>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        SmoothFollow(playerPos);
    }

    private void SmoothFollow(Transform target)
    {
        if (target)
        {
            targetPosition = new Vector3(target.position.x + offsetX, target.position.y + offsetY, target.position.z - 10f);
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref _maxSpeed, smoothTime);
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


