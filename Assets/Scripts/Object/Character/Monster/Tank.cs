using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Tank : BaseMonster
{
    [SerializeField] private Vector3 bulletOffset = new Vector2(1.5f, 0.75f);

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

    protected override void SetAnimatorParameter()
    {
        anim.SetBool("IsFindPlayer", isFindPlayer);
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
        isAttack = true;
        // Debug.Log("Tank攻击");
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "tank_attack");
        GameObject bullet = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("TankBullet", E_ResourcesPath.Object);
        bullet.GetComponent<BaseBullet>().InitBulletPostion(this.transform.position + bulletOffset);

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        isAttack = false;
    }

    public override void Dead()
    {

    }

    private IEnumerator IE_Dead()
    {
        anim.SetTrigger("Dead");
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;
        // GameManager.Instance.m_AudioManager.AudioPlay

        yield return new WaitForSeconds(destroyTime);
    }
}
