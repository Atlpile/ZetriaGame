using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp : BaseMonster
{
    private Vector3 _chaseOffset = new Vector2(0, 0.5f);

    protected override void InitCharacter()
    {
        currentHealth = monsterInfo.maxHealth;
        currentMoveSpeed = monsterInfo.airSpeed;
        rb2D.gravityScale = 0;

        fsm.ChangeState(E_AIState.Idle);
    }

    public override void UpdateAirMove()
    {
        //TODO:使用Lerp移动
        //向Player移动
        this.transform.position = Vector2.MoveTowards(
            this.transform.position,
            player.transform.position + _chaseOffset,
            currentMoveSpeed * Time.deltaTime
        );
    }

    public override void Attack()
    {
        if (!isAttack)
            StartCoroutine(IE_Attack());
    }

    public override void Dead()
    {
        StartCoroutine(IE_Dead());
    }

    private IEnumerator IE_Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        StopMove();

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        ResumeMove();
        isAttack = false;
    }

    private IEnumerator IE_Dead()
    {
        anim.SetTrigger("Dead");
        StopMove();
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "enemy_death_02");

        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

}
