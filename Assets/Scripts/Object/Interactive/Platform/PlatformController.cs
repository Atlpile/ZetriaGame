using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    [HideInInspector] public AudioSource platformAudioSource;
    public List<PlatformButton> buttons = new List<PlatformButton>();
    public GameObject platform;
    public float platformMoveSpeed = 5;

    private void Awake()
    {
        platform = this.transform.GetChild(0).gameObject;
        platformAudioSource = platform.GetComponent<AudioSource>();

        //给所有Button都获得到Controller
        if (buttons.Count > 0)
        {
            foreach (var item in buttons)
            {
                item.RegisterPlatformController(this);
            }
        }
    }

    public void PlatformMove(PlatformButton button)
    {
        if (Vector2.Distance(platform.transform.position, button.targetPoint.position) < 0.1f)
        {
            button.isMax = true;
            button.UpdateButtonLight();
            platformAudioSource.enabled = false;
            //TODO:播放一次结束音效
        }

        platform.transform.position = Vector2.MoveTowards(platform.transform.position, button.targetPoint.position, platformMoveSpeed * Time.deltaTime);

    }

    public void InitAllButtons(PlatformButton button)
    {
        foreach (var item in buttons)
        {
            if (item != button)
            {
                item.isMax = false;
                item.UpdateButtonLight();
            }
        }
    }

}
