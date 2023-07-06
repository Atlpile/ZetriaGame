using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : BaseBullet
{
    protected override void Create()
    {
        anim.Play("Run");
        coll2d.enabled = true;
        ChangeMove(false);
        StartCoroutine(IE_Disappear());
    }

    protected override void InitBullet()
    {
        moveSpeed = 10f;
        currentMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        Move();
    }

    protected override void Explosion()
    {
        //播放爆炸动画
        anim.Play("Explosion");
        //消除Bullet的交互效果
        coll2d.enabled = false;
        ChangeMove(true);
    }

    protected override void Release()
    {
        this.transform.position = Vector2.zero;
        StopAllCoroutines();
    }

    private void Move()
    {
        transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
    }

    private IEnumerator IE_Disappear()
    {
        yield return new WaitForSeconds(explosionTime);
        // Debug.Log("播放爆炸动画");
        Explosion();

        yield return new WaitForSeconds(disappearTime);
        // Debug.Log("子弹消失");
        Hide();
    }

    private IEnumerator IE_HurtPlayer()
    {
        Explosion();

        yield return new WaitForSeconds(disappearTime);
        Hide();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.Damage(this.transform.position);
            StartCoroutine(IE_HurtPlayer());
        }
    }
}
