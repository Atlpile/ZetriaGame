using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamEgg : BaseMonster
{
    protected override void InitCharacter()
    {
        fsm.ChangeState(E_AIState.Idle);
    }

    public override void Attack()
    {

    }

    public override void Dead()
    {

    }


}
