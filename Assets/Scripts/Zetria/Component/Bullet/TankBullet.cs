using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class TankBullet : BaseBullet
    {
        public override void OnInit()
        {
            base.OnInit();
            moveSpeed = 10f;
            currentMoveSpeed = moveSpeed;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            anim.Play("Run");
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.right);
        }


    }

}