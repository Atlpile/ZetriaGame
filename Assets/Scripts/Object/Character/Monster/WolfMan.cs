using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    写显示逻辑
    写专有逻辑
*/

[RequireComponent(typeof(CapsuleCollider2D))]
public class WolfMan : BaseMonster
{
    public override void InitComponent()
    {
        check = this.transform.GetChild(0);
    }

    protected override void InitCharacter()
    {
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

        isFindPlayer = GetPlayer(check.position, monsterInfo.checkSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, monsterInfo.checkSize);
    }
}
