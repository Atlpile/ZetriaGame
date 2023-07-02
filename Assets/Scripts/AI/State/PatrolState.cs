using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatrolState : BaseAIState
{
    public PatrolState(FSM fsm) : base(fsm)
    {
    }

    public override void EnterState()
    {
        Monster.PlayAnim("Move");
        Monster.SetPatrolMove(true);
        Monster.ResumeMove();
    }

    public override void ExitState()
    {
        Monster.SetPatrolMove(false);
    }

    public override void UpdateState()
    {
        Monster.UpdateMove();
        Monster.UpdateFlip();

        if (Monster.IsFindPlayer == true)
        {
            fsm.ChangeState(E_AIState.Idle);
            Debug.Log("发现Player");
        }
    }
}
