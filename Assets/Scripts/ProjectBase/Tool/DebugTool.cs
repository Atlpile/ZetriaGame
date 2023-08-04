using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugTool
{
    public static bool isActive;

    public static void Log(object message)
    {
        if (isActive)
            Debug.Log(message);
    }

    public static void LogWarning(object message)
    {
        if (isActive)
            Debug.LogWarning(message);
    }

    public static void Logerror(object message)
    {
        if (isActive)
            Debug.LogError(message);
    }
}
