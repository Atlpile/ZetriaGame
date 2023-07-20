using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Runner : BaseMonster
{
    protected override void InitCharacter()
    {
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
