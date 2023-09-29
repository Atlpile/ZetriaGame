using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class ShotGunBullet : BaseBullet
    {
        public E_BulletMoveType moveType;
        public float verticalSpeed;

        public override void OnInit()
        {
            base.OnInit();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.freezeRotation = true;
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
                damageable.OnDamage(this.transform.position);
                StartCoroutine(IE_TriggerExplosion());
            }


        }
    }

}
