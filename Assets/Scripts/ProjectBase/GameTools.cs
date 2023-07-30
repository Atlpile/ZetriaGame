using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameTools
{
    public static RaycastHit2D ShowRay(Vector2 rayPos, Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPos + offset, rayDirection, length, layer);
        Color rayColor = hitInfo ? Color.red : Color.green;
        Debug.DrawRay(rayPos + offset, rayDirection * length, rayColor);

        return hitInfo;
    }

    [MenuItem("GameTool/打开数据存储路径")]
    public static void OpenPersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("GameTool/清空所有数据")]
    public static void ClearAllData()
    {

    }

    [MenuItem("GameTool/加载主场景")]
    public static void LoadMainScene()
    {

    }
}
