using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class CharacterBehavior : MonoBehaviour
    {
        private CapsuleCollider2D _col2D;
        private Rigidbody2D _rb2D;

        private void Awake()
        {
            _col2D = this.GetComponent<CapsuleCollider2D>();
            _rb2D = this.GetComponent<Rigidbody2D>();
        }

        public virtual void MoveAndFlip(float horizontalMove, float moveSpeed)
        {
            if (horizontalMove > 0)
            {
                this.transform.Translate(horizontalMove * moveSpeed * Time.deltaTime * Vector2.right);
                this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (horizontalMove < 0)
            {
                this.transform.Translate(horizontalMove * moveSpeed * Time.deltaTime * Vector2.left);
                this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        public virtual void Jump(float jumpForce)
        {
            _rb2D.velocity = new Vector2(0, jumpForce);
        }

        public virtual void Crouch(Vector2 crouchSize, Vector2 crouchOffset)
        {
            _col2D.size = crouchSize;
            _col2D.offset = crouchOffset;
        }

        public virtual void Stand(ZetriaInfo info)
        {
            _col2D.size = info.standSize;
            _col2D.offset = info.standOffset;
        }

        public virtual void StopMove()
        {
            _rb2D.velocity = Vector2.zero;
        }

        public virtual void GetHurtForce(Vector2 attackerPositon)
        {
            if (attackerPositon.x < this.transform.position.x)
                _rb2D.velocity = new Vector2(2, 5);             //向右施加弹力
            else
                _rb2D.velocity = new Vector2(-2, 5);            //向左施加弹力
        }

        public virtual bool GetGround(Vector2 groundCheckPos, float checkRadius)
        {
            if (checkRadius == 0)
                Debug.LogWarning("当前地面检测范围为0");

            if (_rb2D.velocity.y <= 0.1f)
                return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 1 << LayerMask.NameToLayer("Ground"));

            return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 0);
        }

    }
}





