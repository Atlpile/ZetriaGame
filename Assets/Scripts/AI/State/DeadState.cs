using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseAIState
{
    public DeadState(AILogic logic) : base(logic)
    {

    }

    public override void EnterState()
    {
        logic.monster.FSM_Dead();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }
}
