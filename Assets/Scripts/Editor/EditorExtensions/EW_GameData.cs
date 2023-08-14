using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EW_GameData : EditorWindow
{
    [MenuItem("EditorWindow/GameDataWindow")]
    private static void ShowWindow()
    {
        GetWindow<EW_GameData>().Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            // GUILayout.Button("另存为");
            // GUILayout.Label("当前选定数据");
            GUI.Button(new Rect(0, 0, 75, 25), "保存");
            GUI.Button(new Rect(75, 0, 75, 25), "另存为");
            GUI.Label(new Rect(150, 0, 75, 25), "当前选定数据");
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawView()
    {

    }

    //加载数据并映射到Window上
    private void WindowView()
    {
        //获取路径
        //

        //加载json文件
        //解析变量
        //解析值
    }

    private void LoadData()
    {

    }


}
