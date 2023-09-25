using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerComponent : BaseComponent
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private PlayerBehavior _playerBehavior;
        private PlayerInput _playerInput;
        private PlayerAnimation _playerAnimation;
        private PlayerSettingInfo _playerSettingInfo;
        [SerializeField] private PlayerDynamicInfo _playerDynamicInfo;

        private IInputManager _InputManager { get; set; }
        private IObjectPoolManager _ObjectPoolManager { get; set; }
        private IPlayerModel _playerModel;

        private Vector2 _RayOffset
        {
            get
            {
                if (_IsRight)
                    return new Vector2(this._capsuleCollider2D.offset.x, this._capsuleCollider2D.size.y / 2);
                else
                    return new Vector2(-this._capsuleCollider2D.offset.x, this._capsuleCollider2D.size.y / 2);
            }
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


        private void Awake()
        {
            _capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _animator = this.GetComponent<Animator>();

            _InputManager = Manager.GetManager<IInputManager>();
            _ObjectPoolManager = Manager.GetManager<IObjectPoolManager>();
            _playerModel = GameStructure.GetModel<IPlayerModel>();

            _playerSettingInfo = new PlayerSettingInfo();
            _playerDynamicInfo = new PlayerDynamicInfo();

            _playerBehavior = new PlayerBehavior
            (
                this.transform,
                _rigidbody2D,
                _animator,
                _capsuleCollider2D,
                _playerSettingInfo,
                _playerDynamicInfo
            );
            _playerAnimation = new PlayerAnimation(_animator, _rigidbody2D, _playerDynamicInfo);
            _playerInput = new PlayerInput(_InputManager, _playerDynamicInfo);
        }

        private void Start()
        {
            RegisterBehaviorAction();
            RegisterInputAction();

            AddPoolObject();
            InitPlayer();
        }

        private void Update()
        {
            _playerInput.UpdatePlayerInput();
            _playerAnimation.UpdateAnimatorParameter();

            _HorizotalMove = (int)_InputManager.GetAxisRaw("Horizontal");
            _IsGround = UpdateGroundCheck(this.transform.position, 0.15f);
            _CanStand = UpdateStandHeadCheck(_RayOffset, 1f);

            if (_IsGround)
                _playerDynamicInfo.currentJumpCount = _playerSettingInfo.controlInfo.maxJumpCount;
        }

        private void FixedUpdate()
        {

        }

        private void OnDestroy()
        {
            UnRegisterInputAction();
            UnRegisterBehaviorAction();
        }

        private void RegisterBehaviorAction()
        {
            _playerBehavior.OnJumpAction = () => GameStructure.SendCommand(new PlayerJumpCommand(
                fx => SetFXPos(fx, _playerSettingInfo.offsetInfo.jumpFXOffset, _playerSettingInfo.offsetInfo.jumpFXOffset)
            ));
            _playerBehavior.OnMeleeAttackAction = () => GameStructure.SendCommand(new PlayerMeleeAttackCommand(
                fx => SetFXPos(fx, _playerSettingInfo.offsetInfo.kickFXLeftOffset, _playerSettingInfo.offsetInfo.kickFXRightOffset)
            ));
        }

        private void RegisterInputAction()
        {
            _playerInput.Action_MoveAndFlip = _playerBehavior.MoveAndFlip;
            _playerInput.Action_Jump = _playerBehavior.Jump;
            _playerInput.Action_AirJump = _playerBehavior.AirJump;
            _playerInput.Action_Crouch = _playerBehavior.Crouch;
            _playerInput.Action_Stand = _playerBehavior.Stand;

            _playerInput.Action_MeleeAttack = MeleeAttackCoroutine;
        }

        private void UnRegisterBehaviorAction()
        {
            _playerBehavior.OnJumpAction = null;
            _playerBehavior.OnMeleeAttackAction = null;
        }

        private void UnRegisterInputAction()
        {
            _playerInput.Action_MoveAndFlip = null;
            _playerInput.Action_Jump = null;
            _playerInput.Action_AirJump = null;
            _playerInput.Action_Crouch = null;
            _playerInput.Action_Stand = null;
            _playerInput.Action_MeleeAttack = null;
        }

        private void InitPlayer()
        {
            _playerSettingInfo.offsetInfo.standSize = new Vector2(_capsuleCollider2D.size.x, _capsuleCollider2D.size.y);
            _playerSettingInfo.offsetInfo.standOffset = new Vector2(_capsuleCollider2D.offset.x, _capsuleCollider2D.offset.y);
            _playerSettingInfo.offsetInfo.crouchSize = new Vector2(_capsuleCollider2D.size.x, _capsuleCollider2D.size.y / 2);
            _playerSettingInfo.offsetInfo.crouchOffset = new Vector2(_capsuleCollider2D.offset.x, _capsuleCollider2D.offset.y / 2);

            _playerDynamicInfo.stateInfo.isRight = true;
            _playerDynamicInfo.currentHealth = _playerSettingInfo.maxHealth;
            _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.standMoveSpeed;

            _rigidbody2D.gravityScale = _playerSettingInfo.controlInfo.jumpGravity;
            _rigidbody2D.drag = _playerSettingInfo.controlInfo.drag;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }

        private void AddPoolObject()
        {
            _ObjectPoolManager.AddObjects_FromResourcesAsync
            (
                FrameCore.E_ResourcesPath.FX,
                "FX_Jump",
                "FX_Kick"
            );
        }


        #region StartCortine

        private void MeleeAttackCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isMeleeAttack)
                StartCoroutine(_playerBehavior.IE_MeleeAttack());

            StopMove();
        }



        #endregion

        private bool UpdateGroundCheck(Vector3 groundCheckPos, float checkRadius)
        {
            if (checkRadius == 0)
            {
                Debug.LogWarning("当前地面检测范围为0");
                return false;
            }

            //当Player下落时启用地面检测，否则不启用地面检测
            if (_rigidbody2D.velocity.y <= 0.1f)
                return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 1 << LayerMask.NameToLayer("Ground"));

            return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 0);
        }

        private bool UpdateStandHeadCheck(Vector2 rayOffset, float rayLength)
        {
            return
            GameTools.ShowRay(
                this.transform.position,
                rayOffset,
                Vector2.up,
                rayLength,
                1 << LayerMask.NameToLayer("Ground")
            ) ? false : true;
        }

        private void SetFXPos(GameObject fx, Vector3 leftOffset, Vector3 rightOffset)
        {
            if (_IsRight)
                fx.transform.position = this.transform.position + rightOffset;
            else
                fx.transform.position = this.transform.position + leftOffset;

            fx.transform.localRotation = this.transform.localRotation;
        }

        private void SetBulletPos(GameObject bullet, Vector3 leftOffset, Vector3 rightOffset, Vector3 crouchOffset)
        {
            if (_IsRight)
            {
                if (!_playerDynamicInfo.stateInfo.isCrouch)
                    bullet.transform.position = this.transform.position + rightOffset;
                else
                    bullet.transform.position = this.transform.position + rightOffset + crouchOffset;
            }
            else
            {
                if (!_playerDynamicInfo.stateInfo.isCrouch)
                    bullet.transform.position = this.transform.position + leftOffset;
                else
                    bullet.transform.position = this.transform.position + leftOffset + crouchOffset;
            }
            bullet.transform.localRotation = this.transform.localRotation;
        }

        private void StopMove()
        {
            _HorizotalMove = 0;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, 0.15f);
        }


    }
}







