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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        switch (moveType)
        {
            case E_BulletMoveType.Upward:
                transform.Translate(currentMoveSpeed * Time.fixedDeltaTime * new Vector2(1, 0 + verticalSpeed));
                break;
            case E_BulletMoveType.Straight:
                transform.Translate(currentMoveSpeed * Time.fixedDeltaTime * Vector2.right);
                break;
            case E_BulletMoveType.Downward:
                transform.Translate(currentMoveSpeed * Time.fixedDeltaTime * new Vector2(1, 0 - verticalSpeed));
                break;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_ricochet");
            StartCoroutine(IE_TriggerExplosion());
        }

        damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name != "Player")
        {
            damageable.Damage(this.transform.position);
            StartCoroutine(IE_TriggerExplosion());
        }


    }
}
