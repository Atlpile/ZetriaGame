using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PistolBullet : PlayerBullet
    {
        private void FixedUpdate()
        {
            Move();
        }

        public override void SetBulletTransform(bool isRight, bool isCrouch, Transform playerTransform)
        {
            base.SetBulletTransform(isRight, isCrouch, playerTransform);

            if (isRight)
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + info.pistolBulletRightOffset + info.bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + info.pistolBulletRightOffset;
            }
            else
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + info.pistolBulletLeftOffset + info.bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + info.pistolBulletLeftOffset;
            }
        }

        private void Move()
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

