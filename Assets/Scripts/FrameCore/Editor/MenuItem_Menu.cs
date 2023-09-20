using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MenuItem_Menu
{
    [MenuItem("FrameCore/Test")]
    public static void TestMenuItem()
    {
        Debug.Log("MenuItemTest");
    }
}
