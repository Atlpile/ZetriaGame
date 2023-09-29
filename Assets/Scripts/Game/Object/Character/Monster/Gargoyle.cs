using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : BaseMonster
{
    [SerializeField] private Vector3 bulletOffset;

    protected override void InitCharacter()
    {
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "EnemyBullet");
        monsterInfo = GetInfo("SO_Gargoyle");
        rb2D.gravityScale = 0;

        fsm.ChangeState(E_AIState.Idle);
    }

    public override void UpdateAirMove()
    {
        //根据Player位置平移移动
        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) > 0.1f)
        {
            this.transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.right);
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
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_fire");
        GameObject bullet = GameManager.Instance.ObjectPoolManager.GetObject("EnemyBullet");
        // bullet.GetComponent<BaseBullet>().InitBulletPostion(this.transform.position + bulletOffset);
        // bullet.GetComponent<EnemyBullet>().type = E_EnemyBulletType.Down;

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        isAttack = false;
    }

}
