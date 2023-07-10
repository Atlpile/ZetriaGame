using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
    public float volume_BGM;
    public float volume_Effect;
    public bool isFullScreen;
    public bool isBloom;
    public float exposure;

    public SettingData()
    {
        volume_BGM = 0.5f;
        volume_Effect = 0.5f;
        isFullScreen = true;
        isBloom = true;
        exposure = 1f;
    }
}
