using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMan : BaseMonster
{
    protected override void Init()
    {
        aiLogic = new AILogic(this);
        aiLogic.ChangeState(E_AIState.Idle);
    }

}
