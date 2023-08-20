using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public void GetShotGun()
    {
        // GameData gameData = GameManager.Instance.SaveLoadManager.LoadData<GameData>("GameData");
        // if (!gameData.hasShotGun)
        // {
        //     Debug.Log("拾取霰弹枪");
        //     gameData.hasShotGun = true;
        //     GameManager.Instance.SaveLoadManager.SaveData(gameData, "GameData");
        // }

        GameManager.Instance.SaveLoadManager.UpdateData<GameData>(Consts.GameData, data =>
        {
            if (data.hasShotGun == false)
            {
                Debug.Log("拾取霰弹枪");
                data.hasShotGun = true;
            }
        });
    }

    public void GetToken(int id)
    {
        // GameData gameData = GameManager.Instance.SaveLoadManager.LoadData<GameData>("GameData");
        // if (!gameData.TokenContainer.ContainsKey(id.ToString()))
        // {
        //     Debug.Log("拾取令牌");
        //     gameData.TokenContainer.Add(id.ToString(), id);
        //     GameManager.Instance.SaveLoadManager.SaveData(gameData, "GameData");
        // }

        GameManager.Instance.SaveLoadManager.UpdateData<GameData>(Consts.GameData, data =>
        {
            if (!data.TokenContainer.ContainsKey(id.ToString()))
            {
                Debug.Log("拾取令牌");
                data.TokenContainer.Add(id.ToString(), id);
            }
        });

    }
}
