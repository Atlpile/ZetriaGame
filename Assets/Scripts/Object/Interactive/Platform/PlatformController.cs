using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject platform;
    public float platformMoveSpeed = 5;


    private void Awake()
    {
        platform = this.transform.GetChild(0).gameObject;
    }

    public void MovePlatformToward(Transform targetPoint)
    {
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, targetPoint.position, platformMoveSpeed * Time.deltaTime);
    }


}
