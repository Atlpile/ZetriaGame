using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private GameData gameData;

    public void GetShotGun()
    {
        gameData = GameManager.Instance.SaveLoadManager.LoadData<GameData>("GameData");

        if (!gameData.hasShotGun)
        {
            Debug.Log("拾取霰弹枪");
            gameData.hasShotGun = true;
            GameManager.Instance.SaveLoadManager.SaveData(gameData, "GameData");
        }
    }

    public void GetToken(int id)
    {
        gameData = GameManager.Instance.SaveLoadManager.LoadData<GameData>("GameData");

        if (!gameData.TokenContainer.ContainsKey(id.ToString()))
        {
            Debug.Log("拾取令牌");
            gameData.TokenContainer.Add(id.ToString(), id);
            GameManager.Instance.SaveLoadManager.SaveData(gameData, "GameData");
        }

    }
}
