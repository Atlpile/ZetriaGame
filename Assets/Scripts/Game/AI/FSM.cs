using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private E_AIState currentState = E_AIState.Null;
    public BaseMonster monster;

    private readonly Dictionary<E_AIState, BaseAIState> StateDic = new();
    private BaseAIState nowState;


    public FSM(BaseMonster monster)
    {
        this.monster = monster;

        StateDic.Add(E_AIState.Null, new NullState(this));
        StateDic.Add(E_AIState.Idle, new IdleState(this));
        StateDic.Add(E_AIState.Chase, new ChaseState(this));
        StateDic.Add(E_AIState.Attack, new AttackState(this));

        ChangeState(E_AIState.Null);
    }

    public void ChangeState(E_AIState state)
    {
        //旧状态退出
        if (currentState != E_AIState.Null)
            StateDic[currentState].ExitState();

        //切换状态
        if (StateDic.ContainsKey(state))
            currentState = state;
        else
            Debug.LogWarning("未添加该状态,请检查是否添加状态");

        //新状态进入
        StateDic[currentState].EnterState();
        nowState = StateDic[currentState];
    }

    public void UpdateFSM()
    {
        nowState.UpdateState();
    }


}
