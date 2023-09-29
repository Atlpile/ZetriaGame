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
        public Action OnShortGunAttackAction;
        public Action OnEmptyAttackAction;
        public Action OnReloadAction;
        public Action OnDamageAction;
        public Action OnDamageCompleteAction;
        public Action OnDeadAction;


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

        private bool _IsCrouch
        {
            get => _playerDynamicInfo.stateInfo.isCrouch;
            set => _playerDynamicInfo.stateInfo.isCrouch = value;
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

                if (!_IsRight)
                {
                    _playerTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    _IsRight = true;
                }
            }
            else if (_HorizotalMove < 0)
            {
                _playerTransform.Translate(_HorizotalMove * _CurrentMoveSpeed * Time.deltaTime * Vector2.left);
                if (_IsRight)
                {
                    _playerTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    _IsRight = false;
                }
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
            //TODO:Crouch和Stand应该在GetKeyDown 和 GetKeyUp 中执行

            //执行一次Crouch
            if (!_IsCrouch)
            {
                _playerDynamicInfo.stateInfo.isCrouch = true;
                _playerDynamicInfo.status = E_PlayerStatus.Pistol;
                _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.crouchMoveSpeed;

                _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.crouchSize;
                _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.crouchOffset;

                OnCrouchAction?.Invoke();
            }
        }

        public void Stand()
        {
            //TODO:Crouch和Stand应该在GetKeyDown 和 GetKeyUp 中执行

            //执行一次Stand
            if (_IsCrouch)
            {
                _playerDynamicInfo.stateInfo.isCrouch = false;

                if (_playerDynamicInfo.status != E_PlayerStatus.NPC)
                    _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.standMoveSpeed;

                _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.standSize;
                _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.standOffset;
            }
        }

        public void Dead()
        {
            // _moveSource.enabled = false;
            _playerDynamicInfo.currentHealth = 0;

            // StopMove();
            OnDeadAction?.Invoke();

            // GameManager.Instance.EventManager.EventTrigger(E_EventType.PlayerDead);
            // GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(_zetriaInfo.currentHealth, _zetriaInfo.maxHealth);
            // GameManager.Instance.SceneLoader.LoadCurrentSceneInGame();
        }

        private void AddHurtForce(Vector2 attackerPos)
        {
            //FIXME：优化移动效果
            if (attackerPos.x < _playerTransform.position.x)
            {
                //向右施加弹力
                _rigidbody2D.velocity = new Vector2(2, 5);
            }
            else
            {
                //向左施加弹力
                _rigidbody2D.velocity = new Vector2(-2, 5);
            }
        }

        public IEnumerator IE_MeleeAttack()
        {
            _playerDynamicInfo.stateInfo.isMeleeAttack = true;

            _animator.SetTrigger("MeleeAttack");
            _HorizotalMove = 0;
            _rigidbody2D.velocity = Vector2.zero;

            OnMeleeAttackAction?.Invoke();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.meleeAttackCD);
            _playerDynamicInfo.stateInfo.isMeleeAttack = false;
        }

        public IEnumerator IE_PistolAttack()
        {
            _playerDynamicInfo.stateInfo.isPistolAttack = true;

            if (_HorizotalMove == 0)
                _animator.SetTrigger("GunAttack");
            else if (Mathf.Abs(_rigidbody2D.velocity.y) >= 0.1f)
                _animator.SetTrigger("GunAttack");

            OnPistolAttackAction?.Invoke();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.pistolAttackCD);
            _playerDynamicInfo.stateInfo.isPistolAttack = false;
        }

        public IEnumerator IE_ShotGunAttack()
        {
            _playerDynamicInfo.stateInfo.isShotGunAttack = true;

            if (_HorizotalMove == 0)
                _animator.SetTrigger("GunAttack");
            else if (Mathf.Abs(_rigidbody2D.velocity.y) >= 0.1f)
                _animator.SetTrigger("GunAttack");

            OnShortGunAttackAction?.Invoke();

            // ShotGunFire();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.shotGunAttackCD);
            _playerDynamicInfo.stateInfo.isShotGunAttack = false;
        }

        public IEnumerator IE_EmptyAttack()
        {
            _playerDynamicInfo.stateInfo.isEmptyAttack = true;

            OnEmptyAttackAction?.Invoke();
            // GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "gun_empty");

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.emptyAttackCD);
            _playerDynamicInfo.stateInfo.isEmptyAttack = false;
        }

        public IEnumerator IE_Reload()
        {
            _playerDynamicInfo.stateInfo.isReload = true;
            _animator.SetTrigger("Reload");
            _HorizotalMove = 0;
            _rigidbody2D.velocity = Vector2.zero;

            OnReloadAction?.Invoke();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.reloadCD);
            _playerDynamicInfo.stateInfo.isReload = false;
        }

        public IEnumerator IE_Damage(Vector2 attacker)
        {
            _playerDynamicInfo.stateInfo.isHurt = true;

            _animator.SetTrigger("Hurt");
            _playerDynamicInfo.currentHealth = _playerDynamicInfo.currentHealth > 0 ? --_playerDynamicInfo.currentHealth : 0;
            _playerDynamicInfo.stateInfo.isDead = _playerDynamicInfo.currentHealth == 0;
            AddHurtForce(attacker);
            if (_playerDynamicInfo.stateInfo.isDead) Dead();

            OnDamageAction?.Invoke();

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.hurtCD);

            OnDamageCompleteAction?.Invoke();

            _playerDynamicInfo.stateInfo.isHurt = false;
        }
    }
}

