using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFish : BaseMonster
{
    private Vector3 bulletOffset = new Vector2(0, 1);
    private GameObject bullet;
    private float _distanceY;

    protected override void InitCharacter()
    {
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "EnemyBullet");
        monsterInfo = GetInfo("SO_JellyFish");
        rb2D.gravityScale = 0;

        fsm.ChangeState(E_AIState.Idle);
    }

    public override void UpdateAirMove()
    {
        _distanceY = Mathf.Abs(this.transform.position.y - player.position.y);

        if (_distanceY > monsterInfo.attackDistance)
        {
            //到达一定距离时不移动 
            this.transform.position = Vector2.Lerp(this.transform.position, player.position, monsterInfo.airSpeed * Time.deltaTime);
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

    private void Fire()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_fire");
        bullet = GameManager.Instance.ObjectPoolManager.GetObject("EnemyBullet");
        // bullet.GetComponent<BaseBullet>().InitBulletPostion(this.transform.position + bulletOffset);
        // bullet.GetComponent<EnemyBullet>().type = E_EnemyBulletType.Horizontal;

        if (isRight == false)
            bullet.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            bullet.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator IE_Attack()
    {
        isAttack = true;

        Fire();

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        isAttack = false;
    }

}
