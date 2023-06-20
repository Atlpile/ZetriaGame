using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleState : BaseAIState
{
    public IdleState(FSM fsm) : base(fsm) { }

    public override void EnterState()
    {
        //播放Idle动画
        Monster.PlayAnim("Idle");

        //停止移动

    }

    public override void UpdateState()
    {
        //若发现Player，则切换为Chase状态
        if (Monster.IsFindPlayer)
        {
            fsm.ChangeState(E_AIState.Chase);
        }
        else if (Monster is WolfMan)
        {

        }
    }

    public override void ExitState()
    {

    }
}

