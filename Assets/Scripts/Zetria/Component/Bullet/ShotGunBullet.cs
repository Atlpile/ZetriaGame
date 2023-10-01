using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class ShotGunBullet : PlayerBullet
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

        public override void SetBulletTransform(bool isRight, bool isCrouch, Transform playerTransform)
        {
            base.SetBulletTransform(isRight, isCrouch, playerTransform);

            if (isRight)
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + info.shortGunBulletRightOffset + info.bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + info.shortGunBulletRightOffset;
            }
            else
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + info.shortGunBulletLeftOffset + info.bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + info.shortGunBulletLeftOffset;
            }
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
