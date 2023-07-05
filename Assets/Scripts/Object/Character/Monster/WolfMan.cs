using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    写显示逻辑
    写专有逻辑
*/

public class WolfMan : BaseMonster
{
    protected override void InitCharacter()
    {
        monsterInfo.monsterType = E_MonsterType.Ground;
        monsterInfo.attackDuration = 1.5f;
        monsterInfo.attackDistance = 1f;
        monsterInfo.checkSize = new Vector2(7, 2);
        monsterInfo.groundSpeed = 3f;
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
        Gizmos.DrawWireCube(check.position, monsterInfo.checkSize);
    }
}
