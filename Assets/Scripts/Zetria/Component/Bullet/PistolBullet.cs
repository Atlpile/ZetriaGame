using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PistolBullet : PlayerBullet
    {
        public override void OnInit()
        {
            base.OnInit();

            bulletLeftOffset = new Vector2(-1f, 1.15f);
            bulletRightOffset = new Vector2(1f, 1.15f);
        }

        protected override void Move()
        {
            transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.right);
        }


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null && other.gameObject.name != "Player")
            {
                damageable.OnDamage(this.transform.position);
                StartCoroutine(IE_TriggerExplosion());
            }

            if (other.CompareTag("Ground"))
            {
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_ricochet");
                StartCoroutine(IE_TriggerExplosion());
            }
        }
    }
}

