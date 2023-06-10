using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    public GameObject platform;
    public float platformMoveSpeed = 5;

    private void Awake()
    {
        platform = this.transform.GetChild(0).gameObject;
    }

    public void PlatformMove(Transform targetPoint)
    {
        platform.GetComponent<Platform>().Move(targetPoint, platformMoveSpeed);
        platform.GetComponent<Platform>().UpdateAudio(true);
    }

    public void PlatformStop()
    {
        platform.GetComponent<Platform>().UpdateAudio(false);
    }
}
