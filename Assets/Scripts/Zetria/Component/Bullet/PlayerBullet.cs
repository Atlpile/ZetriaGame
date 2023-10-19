using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerBullet : BaseBullet
    {
        protected Vector3 bulletLeftOffset;
        protected Vector3 bulletRightOffset;
        protected Vector3 bulletOffsetWithCrouch = new Vector2(0, -0.5f);


        private void FixedUpdate()
        {
            Move();
        }

        public override void OnInit()
        {
            base.OnInit();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.freezeRotation = true;

            // info = new PlayerOffsetInfo();
        }

        public void SetBulletTransform(bool isRight, bool isCrouch, Transform playerTransform)
        {
            if (isRight)
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + bulletRightOffset + bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + bulletRightOffset;
            }
            else
            {
                if (isCrouch)
                    this.transform.position = playerTransform.position + bulletLeftOffset + bulletOffsetWithCrouch;
                else
                    this.transform.position = playerTransform.position + bulletLeftOffset;
            }

            this.transform.localRotation = playerTransform.rotation;
        }

        protected virtual void Move()
        {

        }
    }
}


