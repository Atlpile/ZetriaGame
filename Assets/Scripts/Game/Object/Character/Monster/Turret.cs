using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BaseMonster
{
    [SerializeField] private Vector3 bulletOffset = new Vector2(1.5f, 0.75f);


    protected override void InitCharacter()
    {
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "TankBullet");
        monsterInfo = GetInfo("SO_Turret");
        hasAttackForce = false;
        destroyTime = 0.5f;

        fsm.ChangeState(E_AIState.Idle);
    }

    protected override void SetAnimatorParameter()
    {
        anim.SetBool("IsFindPlayer", isFindPlayer);
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

        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "tank_attack");
        GameObject bullet = GameManager.Instance.ObjectPoolManager.GetObject("TankBullet");
        bullet.GetComponent<BaseBullet>().InitBulletPostion(this.transform.position + bulletOffset);

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        isAttack = false;
    }

    private IEnumerator IE_Dead()
    {
        anim.SetTrigger("Dead");
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_blast");

        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        BaseBullet baseBullet = other.gameObject.GetComponent<BaseBullet>();
        if (baseBullet is PistolBullet || baseBullet is ShotGunBullet)
        {
            baseBullet.Hide();
        }
    }
}
