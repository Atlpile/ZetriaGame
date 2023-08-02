using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Dictionary<string, int> TokenContainer = new Dictionary<string, int>();
    public bool hasShotGun;

    public GameData()
    {
        hasShotGun = false;
    }
}
