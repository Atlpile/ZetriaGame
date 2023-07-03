using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Tank : BaseMonster
{
    protected override void InitCharacter()
    {
        monsterInfo.monsterType = E_MonsterType.Static;
        monsterInfo.checkSize = new Vector2(7, 2);
        monsterInfo.checkOffset = new Vector2(3.5f, 1);
        monsterInfo.groundSpeed = 0;
        monsterInfo.currentHealth = 1;

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

    public override void Attack()
    {
        if (!isAttack)
            StartCoroutine(IE_Attack());
    }

    private IEnumerator IE_Attack()
    {
        yield return new WaitForSeconds(monsterInfo.attackDuration);
    }
}
