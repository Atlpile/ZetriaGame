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
        Debug.Log("Tank进入AttackState");
    }

    public override void ExitState()
    {
        Debug.Log("Tank离开AttackState");
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
