using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AILogic
{
    public E_AIState currentState = E_AIState.Null;
    public BaseMonster monster;

    private Dictionary<E_AIState, BaseAIState> StateDic = new Dictionary<E_AIState, BaseAIState>();
    private BaseAIState nowState;


    public AILogic(BaseMonster monster)
    {
        this.monster = monster;

        StateDic.Add(E_AIState.Patrol, new PatrolState(this));
        StateDic.Add(E_AIState.Chase, new ChaseState(this));
    }


    public void UpdateState()
    {
        nowState.UpdateState();
    }

    public void ChangeState(E_AIState state)
    {
        if (currentState != E_AIState.Null)
            StateDic[currentState].ExitState();

        this.currentState = state;

        StateDic[currentState].EnterState();
        nowState = StateDic[currentState];



    }
}
