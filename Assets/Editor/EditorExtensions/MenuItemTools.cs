using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MenuItemTools
{
    [MenuItem("EditorTool/打开数据存储路径")]
    public static void OpenPersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("EditorTool/清空所有数据")]
    public static void ClearAllData()
    {

    }

    [MenuItem("EditorTool/加载主场景")]
    public static void LoadMainScene()
    {

    }

    [MenuItem("EditorTool/选中文件夹/Resources")]
    public static void SelectResourceFolder()
    {
        //加载想要选中的文件/文件夹
        var folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources");
        //在Project面板标记高亮显示
        EditorGUIUtility.PingObject(folder);
        //在Project面板自动选中，并在Inspector面板显示详情
        Selection.activeObject = folder;
    }

    [MenuItem("EditorTool/选中文件夹/SceneLevel")]
    public static void SelectSceneFolder()
    {
        var folder = AssetDatabase.LoadAssetAtPath<Object>("Assets/Scenes/Level");
        EditorGUIUtility.PingObject(folder);
        Selection.activeObject = folder;
    }
}
