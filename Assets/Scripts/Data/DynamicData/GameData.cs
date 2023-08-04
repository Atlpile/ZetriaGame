using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Dictionary<string, int> TokenContainer = new();
    public bool hasShotGun;

    public GameData()
    {
        hasShotGun = false;
    }
}
