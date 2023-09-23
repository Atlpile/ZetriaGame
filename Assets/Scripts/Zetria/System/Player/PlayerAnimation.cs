using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerAnimation
    {
        private Animator anim;
        private Rigidbody2D rb2D;
        private PlayerDynamicInfo dynamicInfo;

        private const string P_PlayerStatus = "PlayerStatus";
        private const string P_Horizontal = "Horizontal";
        private const string P_Vertical = "Vertical";
        private const string P_IsGround = "IsGround";
        private const string P_IsCrouch = "IsCrouch";
        private const string P_IsDead = "IsDead";


        public PlayerAnimation(Animator animator, Rigidbody2D rigidbody2D, PlayerDynamicInfo info)
        {
            this.anim = animator;
            this.rb2D = rigidbody2D;
            this.dynamicInfo = info;
        }

        public void UpdateAnimatorParameter()
        {
            anim.SetFloat(P_Vertical, rb2D.velocity.y);
            anim.SetInteger(P_Horizontal, dynamicInfo.horizontalMove);
            anim.SetInteger(P_PlayerStatus, (int)dynamicInfo.status);
            anim.SetBool(P_IsGround, dynamicInfo.stateInfo.isGround);
            anim.SetBool(P_IsCrouch, dynamicInfo.stateInfo.isCrouch);
            anim.SetBool(P_IsDead, dynamicInfo.stateInfo.isDead);
        }
    }
}


