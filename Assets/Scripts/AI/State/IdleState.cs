using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleState : BaseAIState
{
    public IdleState(FSM fsm) : base(fsm) { }

    public override void EnterState()
    {
        //停止移动
        Monster.StopMove();
    }

    public override void ExitState()
    {
        //恢复移动
        Monster.ResumeMove();
    }

    public override void UpdateState()
    {
        //若发现Player，则切换为Chase状态
        if (Monster.IsFindPlayer == true)
        {
            fsm.ChangeState(E_AIState.Chase);
            // Debug.Log("追击Player");
        }
    }


}

