using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    public GameData gameData;
    public SettingData settingData;

    public GameDataManager()
    {
        gameData = new GameData();
        settingData = new SettingData();
    }
}
