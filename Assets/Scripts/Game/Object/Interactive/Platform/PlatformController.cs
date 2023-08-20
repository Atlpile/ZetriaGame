using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    public GameObject module_platform;
    public GameObject module_button;
    public float platformMoveSpeed = 5;

    private Platform _platform;
    private PlatformButton[] _buttons;

    private void Awake()
    {
        _platform = module_platform.GetComponent<Platform>();
        _buttons = module_button.GetComponentsInChildren<PlatformButton>();

        foreach (var button in _buttons)
        {
            button.GetPlatform(_platform);
        }
    }

    private void Start()
    {
        _platform.SetMoveSpeed(platformMoveSpeed);
    }
}
