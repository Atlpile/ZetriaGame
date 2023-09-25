using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class PlayerAnimation
    {
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private PlayerDynamicInfo _playerDynamicInfo;

        private const string P_PlayerStatus = "PlayerStatus";
        private const string P_Horizontal = "Horizontal";
        private const string P_Vertical = "Vertical";
        private const string P_IsGround = "IsGround";
        private const string P_IsCrouch = "IsCrouch";
        private const string P_IsDead = "IsDead";


        public PlayerAnimation(Animator animator, Rigidbody2D rigidbody2D, PlayerDynamicInfo info)
        {
            this._animator = animator;
            this._rigidbody2D = rigidbody2D;
            this._playerDynamicInfo = info;
        }

        public void UpdateAnimatorParameter()
        {
            _animator.SetFloat(P_Vertical, _rigidbody2D.velocity.y);
            _animator.SetInteger(P_Horizontal, _playerDynamicInfo.horizontalMove);
            _animator.SetInteger(P_PlayerStatus, (int)_playerDynamicInfo.status);
            _animator.SetBool(P_IsGround, _playerDynamicInfo.stateInfo.isGround);
            _animator.SetBool(P_IsCrouch, _playerDynamicInfo.stateInfo.isCrouch);
            _animator.SetBool(P_IsDead, _playerDynamicInfo.stateInfo.isDead);
        }
    }
}


