using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NullState : BaseAIState
{
    public NullState(FSM fsm) : base(fsm)
    {
    }

    public override void EnterState()
    {
        DebugTool.Log("Enemy处于Null状态");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }
}
