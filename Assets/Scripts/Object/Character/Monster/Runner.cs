using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Runner : BaseMonster
{
    protected override void InitCharacter()
    {
        monsterInfo.attackDuration = 1.5f;
        monsterInfo.attackDistance = 1f;
        monsterInfo.checkSize = new Vector2(7, 2);
        monsterInfo.checkOffset = new Vector2(0, 0.75f);
        monsterInfo.groundSpeed = 4f;
        monsterInfo.currentHealth = monsterInfo.maxHealth;

        currentMoveSpeed = monsterInfo.groundSpeed;
        fsm.ChangeState(E_AIState.Idle);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        isFindPlayer = GetPlayer(check.position + monsterInfo.checkOffset, monsterInfo.checkSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position + monsterInfo.checkOffset, monsterInfo.checkSize);
    }
}
