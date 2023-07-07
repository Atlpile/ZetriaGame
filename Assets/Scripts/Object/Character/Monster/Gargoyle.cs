using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : BaseMonster
{
    protected override void InitCharacter()
    {
        monsterInfo.monsterType = E_MonsterType.Fly;
        monsterInfo.groundSpeed = 0;
        monsterInfo.airSpeed = 2f;

        currentMoveSpeed = monsterInfo.airSpeed;
        rb2D.gravityScale = 0;

        fsm.ChangeState(E_AIState.Idle);
    }

    protected override void OnUpdate()
    {
        isFindPlayer = GetPlayer(this.transform.position, monsterInfo.checkRadius);
    }

    protected override void SetAnimatorParameter()
    {
        anim.SetBool(Consts.Anim_IsFindPlayer, isFindPlayer);
    }

    public override void UpdateAirMove()
    {
        //根据Player位置平移移动
        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) > 0.1f)
        {
            this.transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
        }
    }

    public override void Attack()
    {
        //发射水滴向下攻击
        // Debug.Log("攻击");
    }

    public override void Dead()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position + monsterInfo.checkOffset, monsterInfo.checkRadius);
    }


}
