using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    TODO:写AI逻辑
    TODO:使用里氏替换原则为不同的Monster写逻辑
*/


public abstract class BaseAIState
{
    protected FSM fsm;
    protected BaseMonster Monster => fsm.monster;

    public BaseAIState(FSM fsm)
    {
        this.fsm = fsm;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();
}


