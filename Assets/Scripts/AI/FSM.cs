using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSM
{
    private E_AIState currentState = E_AIState.Null;
    public BaseMonster monster;

    private Dictionary<E_AIState, BaseAIState> StateDic = new Dictionary<E_AIState, BaseAIState>();
    private BaseAIState nowState;


    public FSM(BaseMonster monster)
    {
        this.monster = monster;

        //OPTIMIZE：自动添加状态
        StateDic.Add(E_AIState.Null, new NullState(this));
        StateDic.Add(E_AIState.Idle, new IdleState(this));
        StateDic.Add(E_AIState.Patrol, new PatrolState(this));
        StateDic.Add(E_AIState.Chase, new ChaseState(this));
        StateDic.Add(E_AIState.Attack, new AttackState(this));

        ChangeState(E_AIState.Null);
    }

    public void ChangeState(E_AIState state)
    {
        //旧状态退出
        if (currentState != E_AIState.Null)
            StateDic[currentState].ExitState();

        if (StateDic.ContainsKey(state))
        {
            currentState = state;
        }


        //新状态进入
        StateDic[currentState].EnterState();
        nowState = StateDic[currentState];
    }

    public void UpdateFSM()
    {
        nowState.UpdateState();
    }


}
