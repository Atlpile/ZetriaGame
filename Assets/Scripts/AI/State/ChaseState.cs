using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChaseState : BaseAIState
{
    public ChaseState(FSM fsm) : base(fsm) { }

    public override void EnterState()
    {
        //切换到跑步动画
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //向Player移动
        if (Monster.MonsterInfo.monsterType == E_MonsterType.Fly)
        {
            Monster.UpdateAirMove();
        }
        else
        {
            Monster.UpdateGroundMove();
        }


        Monster.UpdateFlip();
        Monster.FlipToPlayer();

        //到一定距离时停下并攻击Player
        if (Monster.CanAttack())
        {
            Monster.Attack();
        }

        //追击Player
        if (Monster.IsFindPlayer == false && Monster.IsAttack == false)
        {
            fsm.ChangeState(E_AIState.Idle);
            // Debug.Log("Player脱离");
        }
    }
}
