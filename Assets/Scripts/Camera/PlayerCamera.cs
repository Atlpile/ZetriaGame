using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Camera mainCamera;

    [Header("平滑设置")]
    public Transform playerPos;
    public float offsetX;
    public float offsetY = 1f;
    public float smoothTime = 0.15f;

    private Vector3 _maxSpeed = Vector3.zero;
    private Vector3 targetPosition;

    private void Awake()
    {
        mainCamera = this.GetComponent<Camera>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        SmoothCamera();
    }

    private void SmoothCamera()
    {
        if (playerPos)
        {
            targetPosition = new Vector3(playerPos.position.x + offsetX, playerPos.position.y + offsetY, playerPos.position.z - 10f);
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref _maxSpeed, smoothTime);
        }
    }

}


