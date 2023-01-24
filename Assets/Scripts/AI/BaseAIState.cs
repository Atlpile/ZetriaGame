using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAIState
{
    protected AILogic logic;

    public BaseAIState(AILogic logic)
    {
        this.logic = logic;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();
}


