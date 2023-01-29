using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [HideInInspector] public GameObject platform;
    public float moveSpeed = 5;


    private void Awake()
    {
        platform = this.transform.GetChild(0).gameObject;
    }

    public void PlatformMove(Transform targetPoint)
    {
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
    }


}
