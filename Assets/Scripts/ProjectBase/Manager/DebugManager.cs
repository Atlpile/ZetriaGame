using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager
{
    public bool canActiveDebugger;

    public DebugManager()
    {
        canActiveDebugger = true;
    }

    public void SetActiveDebugger(bool isActive)
    {
        Debug.developerConsoleVisible = isActive;
    }
}
