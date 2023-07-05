using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : BaseAIState
{
    public AttackState(FSM fsm) : base(fsm)
    {
    }

    public override void EnterState()
    {
        if (Monster is Egg)
        {
            Monster.Attack();
        }
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (Monster is Tank)
        {
            Monster.Attack();
        }


        if (Monster.IsFindPlayer == false)
        {
            fsm.ChangeState(E_AIState.Idle);
        }
    }
}
