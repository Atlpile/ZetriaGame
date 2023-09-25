using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class PlayerBehavior
    {
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private CapsuleCollider2D _capsuleCollider2D;
        private PlayerSettingInfo _playerSettingInfo;
        private PlayerDynamicInfo _playerDynamicInfo;

        public Action OnJumpAction;
        public Action OnCrouchAction;
        public Action OnMeleeAttackAction;
        public Action OnPistolAttackAction;


        public PlayerBehavior
        (
            Transform playerTransform,
            Rigidbody2D rigidbody2D,
            Animator animator,
            CapsuleCollider2D capsuleCollider2D,
            PlayerSettingInfo playerSettingInfo,
            PlayerDynamicInfo playerDynamicInfo
        )
        {
            this._playerTransform = playerTransform;
            this._rigidbody2D = rigidbody2D;
            this._animator = animator;
            this._capsuleCollider2D = capsuleCollider2D;
            this._playerSettingInfo = playerSettingInfo;
            this._playerDynamicInfo = playerDynamicInfo;
        }

        private bool _IsGround
        {
            get => _playerDynamicInfo.stateInfo.isGround;
            set => _playerDynamicInfo.stateInfo.isGround = value;
        }

        private bool _IsRight
        {
            get => _playerDynamicInfo.stateInfo.isRight;
            set => _playerDynamicInfo.stateInfo.isRight = value;
        }

        private bool _CanStand
        {
            get => _playerDynamicInfo.stateInfo.canStand;
            set => _playerDynamicInfo.stateInfo.canStand = value;
        }

        private int _HorizotalMove
        {
            get => _playerDynamicInfo.horizontalMove;
            set => _playerDynamicInfo.horizontalMove = value;
        }

        private float _CurrentMoveSpeed
        {
            get => _playerDynamicInfo.currentMoveSpeed;
            set => _playerDynamicInfo.currentMoveSpeed = value;
        }

        public void MoveAndFlip()
        {
            if (_HorizotalMove > 0)
            {
                _playerTransform.Translate(_HorizotalMove * _CurrentMoveSpeed * Time.deltaTime * Vector2.right);
                _playerTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                _IsRight = true;
            }
            else if (_HorizotalMove < 0)
            {
                _playerTransform.Translate(_HorizotalMove * _CurrentMoveSpeed * Time.deltaTime * Vector2.left);
                _playerTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                _IsRight = false;
            }
        }

        public void Jump()
        {
            _rigidbody2D.velocity = new Vector2(0f, _playerSettingInfo.controlInfo.jumpForce);

            OnJumpAction?.Invoke();
        }

        public void AirJump()
        {
            Jump();
            _playerDynamicInfo.currentJumpCount--;
        }

        public void Crouch()
        {
            _playerDynamicInfo.stateInfo.isCrouch = true;
            _playerDynamicInfo.status = E_PlayerStatus.Pistol;
            _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.crouchMoveSpeed;

            _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.crouchSize;
            _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.crouchOffset;

            OnCrouchAction?.Invoke();

            // GamePanel gamePanel = GameManager.Instance.UIManager.GetExistPanel<GamePanel>();
            // if (gamePanel != null)
            //     gamePanel.UpdateAmmoPointer(_status == 0);
        }

        public void Stand()
        {
            _playerDynamicInfo.stateInfo.isCrouch = false;

            if (_playerDynamicInfo.status != E_PlayerStatus.NPC)
                _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.standMoveSpeed;

            _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.standSize;
            _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.standOffset;
        }

        public IEnumerator IE_MeleeAttack()
        {
            _playerDynamicInfo.stateInfo.isMeleeAttack = true;

            _animator.SetTrigger("MeleeAttack");

            OnMeleeAttackAction?.Invoke();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.meleeAttackCD);
            _playerDynamicInfo.stateInfo.isMeleeAttack = false;
        }

        private IEnumerator IE_PistolAttack()
        {
            _playerDynamicInfo.stateInfo.isPistolAttack = true;

            if (_HorizotalMove == 0)
                _animator.SetTrigger("GunAttack");
            else if (Mathf.Abs(_rigidbody2D.velocity.y) >= 0.1f)
                _animator.SetTrigger("GunAttack");

            OnPistolAttackAction?.Invoke();

            // PistolFire();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.pistolAttackCD);
            _playerDynamicInfo.stateInfo.isPistolAttack = false;
        }
    }
}

