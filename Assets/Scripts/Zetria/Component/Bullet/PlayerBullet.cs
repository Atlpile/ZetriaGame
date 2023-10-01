using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerBullet : BaseBullet
    {
        protected PlayerOffsetInfo info;

        public override void OnInit()
        {
            base.OnInit();
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.freezeRotation = true;

            info = new PlayerOffsetInfo();
        }

        public virtual void SetBulletTransform(bool isRight, bool isCrouch, Transform playerTransform)
        {
            this.transform.localRotation = playerTransform.rotation;
        }
    }
}


