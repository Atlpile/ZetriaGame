using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseAIState
{
    public IdleState(AILogic logic) : base(logic)
    {
    }

    public override void EnterState()
    {
        logic.monster.FSM_Idle();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //检测是否发现Player
        if (logic.monster.GetPlayer())
            logic.ChangeState(E_AIState.Chase);
    }

    //发现Player，切换到Chase状态
}

