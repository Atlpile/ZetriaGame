using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : BaseMonster
{
    [SerializeField] private Vector3 bulletOffset;

    protected override void InitCharacter()
    {
        monsterInfo = GetInfo("SO_Gargoyle");
        rb2D.gravityScale = 0;

        fsm.ChangeState(E_AIState.Idle);
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
        if (!isAttack)
            StartCoroutine(IE_Attack());
    }

    public override void Dead()
    {
        StartCoroutine(IE_BaseDead());
    }

    private IEnumerator IE_Attack()
    {
        isAttack = true;

        anim.SetTrigger("Attack");
        GameObject bullet = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("EnemyBullet", E_ResourcesPath.Object);
        bullet.GetComponent<BaseBullet>().InitBulletPostion(this.transform.position + bulletOffset);
        bullet.GetComponent<EnemyBullet>().type = E_EnemyBulletType.Vertical;

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        isAttack = false;
    }

}
