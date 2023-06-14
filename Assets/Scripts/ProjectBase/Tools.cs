using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    private static Tools instance = new Tools();
    public static Tools Instance => instance;
    private Tools()
    {

    }

    //存储大图对应的小图资源信息
    private Dictionary<string, Dictionary<string, Sprite>> dic = new Dictionary<string, Dictionary<string, Sprite>>();


    /// <summary>
    /// 获取Multiple图集中的某一张小图
    /// </summary>
    /// <param name="multipleName">图集名</param>
    /// <param name="spriteName">单张图片名</param>
    /// <returns></returns>
    public Sprite LoadMultipleSprite(string multipleName, string spriteName)
    {
        //判断是否加载过该大图
        if (dic.ContainsKey(multipleName))
        {
            //判断大图中是否有该小图信息
            if (dic[multipleName].ContainsKey(spriteName))
            {
                return dic[multipleName][spriteName];
            }
        }
        else
        {
            Dictionary<string, Sprite> dicTemp = new Dictionary<string, Sprite>();
            Sprite[] sprites = Resources.LoadAll<Sprite>(multipleName);
            for (int i = 0; i < sprites.Length; i++)
            {
                dicTemp.Add(sprites[i].name, sprites[i]);
            }

            dic.Add(multipleName, dicTemp);

            //判断是否有该名字的小图
            if (dicTemp.ContainsKey(spriteName))
                return dicTemp[spriteName];
        }
        return null;
    }

    public void ClearInfo()
    {
        dic.Clear();

        Resources.UnloadUnusedAssets();
    }
}
