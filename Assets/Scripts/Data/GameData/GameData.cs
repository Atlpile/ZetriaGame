using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool hasShotGun;
    public Dictionary<int, Token> TokenDic;

    public GameData()
    {
        hasShotGun = false;
        TokenDic = new Dictionary<int, Token>();
    }
}
