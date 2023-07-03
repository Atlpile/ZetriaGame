using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleState : BaseAIState
{
    public IdleState(FSM fsm) : base(fsm) { }

    public override void EnterState()
    {
        //静态类型的怪物不移动
        if (Monster.MonsterInfo.monsterType != E_MonsterType.Static)
        {
            //可动类型的怪物初始时停止移动
            Monster.StopMove();
        }
    }

    public override void ExitState()
    {
        //同上
        if (Monster.MonsterInfo.monsterType != E_MonsterType.Static)
        {
            //恢复移动
            Monster.ResumeMove();
        }
    }

    public override void UpdateState()
    {
        //若发现Player，则切换为Chase状态
        if (Monster.IsFindPlayer == true)
        {
            if (Monster is Tank)
            {
                fsm.ChangeState(E_AIState.Attack);
            }
            else
            {
                fsm.ChangeState(E_AIState.Chase);
            }
        }
    }


}

