using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBullet : BaseBullet
{
    public E_BulletMoveType moveType;
    public float verticalSpeed;


    protected override void InitBullet()
    {
        base.InitBullet();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;
    }


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (moveType)
        {
            case E_BulletMoveType.Upward:
                transform.Translate(new Vector2(1, 0 + verticalSpeed) * Time.deltaTime * currentMoveSpeed);
                break;
            case E_BulletMoveType.Straight:
                transform.Translate(Vector2.right * Time.deltaTime * currentMoveSpeed);
                break;
            case E_BulletMoveType.Downward:
                transform.Translate(new Vector2(1, 0 - verticalSpeed) * Time.deltaTime * currentMoveSpeed);
                break;
        }
    }

    //FIXME:ShortGunBullet子弹总是会穿墙
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hurtTarget = other.gameObject.GetComponent<IDamageable>();
        if (hurtTarget != null && other.gameObject.name != "Player")
        {
            hurtTarget.Damage(this.transform.position);
            Hide();
        }

        if (other.gameObject.name == "Ground")
        {
            Debug.Log("子弹撞墙");
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_ricochet");
            Hide();
        }
    }
}
