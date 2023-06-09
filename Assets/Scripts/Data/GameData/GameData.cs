using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public bool hasShotGun;
    public Dictionary<string, int> TokenDic;

    public GameData()
    {
        hasShotGun = false;
        TokenDic = new Dictionary<string, int>();
    }
}
