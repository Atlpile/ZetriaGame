using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Runner : BaseMonster
{
    protected override void InitCharacter()
    {
        fsm.ChangeState(E_AIState.Idle);
    }
}
