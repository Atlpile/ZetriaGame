using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private GameData gameData;

    public void GetShotGun()
    {
        gameData = GameManager.Instance.m_SaveLoadManager.LoadData<GameData>("GameData");

        if (!gameData.hasShotGun)
        {
            Debug.Log("拾取霰弹枪");
            gameData.hasShotGun = true;
            GameManager.Instance.m_SaveLoadManager.SaveData(gameData, "GameData");
        }
    }

    public void GetToken(int id)
    {
        gameData = GameManager.Instance.m_SaveLoadManager.LoadData<GameData>("GameData");

        if (!gameData.TokenDic.ContainsKey(id.ToString()))
        {
            Debug.Log("拾取令牌");
            gameData.TokenDic.Add(id.ToString(), id);
            GameManager.Instance.m_SaveLoadManager.SaveData(gameData, "GameData");
        }

    }
}
