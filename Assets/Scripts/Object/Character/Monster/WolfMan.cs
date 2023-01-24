using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMan : BaseMonster
{
    public override void Init()
    {
        aiLogic = new AILogic(this);
        aiLogic.ChangeState(E_AIState.Patrol);

        rb2D.freezeRotation = true;
    }
}
