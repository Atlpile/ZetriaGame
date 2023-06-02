using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public GameData GameData
    {
        get => GameManager.Instance.gameData;
        set { GameManager.Instance.gameData = value; }
    }

    public ItemManager()
    {
        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShotGun, GetShotGun);
        // GameManager.Instance.m_EventManager.AddEventListener<Token>(E_EventType.PickUpToken, GetToken);
    }

    public void GetShotGun()
    {
        GameData.hasShotGun = true;
        GameManager.Instance.m_BinaryDataManager.SaveData(typeof(GameData).Name, GameData);
    }

    public void GetToken(Token token)
    {
        GameData.TokenDic.Add(token.id, token);
    }


}
