using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
    public float volume;
    public bool isFullScreen;
    public bool isBloom;
    public float exposure;

    public SettingData()
    {
        volume = 1f;
        isFullScreen = true;
        isBloom = true;
        exposure = 1f;
    }
}
