using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BaseBullet
{
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
        transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name != "Player")
        {
            damageable.Damage(this.transform.position);
            StartCoroutine(IE_TriggerExplosion());
        }

        if (other.CompareTag("Ground"))
        {
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_ricochet");
            StartCoroutine(IE_TriggerExplosion());
        }
    }
}
