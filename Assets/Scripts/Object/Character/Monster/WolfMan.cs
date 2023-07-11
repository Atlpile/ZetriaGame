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

        currentHealth = monsterInfo.maxHealth;
        currentMoveSpeed = monsterInfo.groundSpeed;
        rb2D.drag = 3f;
        rb2D.gravityScale = 3f;
        fsm.ChangeState(E_AIState.Idle);
    }

    protected override void SetAnimatorParameter()
    {
        anim.SetBool("IsFindPlayer", isFindPlayer);
        anim.SetBool("IsAttack", isAttack);
    }

    public override void Attack()
    {
        if (!isAttack)
            StartCoroutine(IE_BaseAttack());
    }

    public override void Dead()
    {
        StartCoroutine(IE_BaseDead());
    }
}
