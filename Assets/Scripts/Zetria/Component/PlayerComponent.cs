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

        private PlayerInput _playerInput;
        private PlayerAnimation _playerAnimation;
        private PlayerSettingInfo _playerSettingInfo;
        [SerializeField] private PlayerDynamicInfo _playerDynamicInfo;

        private IInputManager _inputManager;
        private IObjectPoolManager _objectPoolManager;
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

        private float _CurrentMoveSpeed
        {
            get => _playerDynamicInfo.currentMoveSpeed;
            set => _playerDynamicInfo.currentMoveSpeed = value;
        }

        private void Awake()
        {
            _capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();
            _rigidbody2D = this.GetComponent<Rigidbody2D>();
            _animator = this.GetComponent<Animator>();

            _inputManager = Manager.GetManager<IInputManager>();
            _playerModel = GameStructure.GetModel<IPlayerModel>();

            _playerSettingInfo = new PlayerSettingInfo();
            _playerDynamicInfo = new PlayerDynamicInfo();
            _playerAnimation = new PlayerAnimation(_animator, _rigidbody2D, _playerDynamicInfo);
            _playerInput = new PlayerInput(_inputManager, _playerDynamicInfo);
        }

        private void Start()
        {
            RegisterInputAction();

            InitPlayer();
        }

        private void Update()
        {
            _playerInput.UpdatePlayerInput();
            _playerAnimation.UpdateAnimatorParameter();

            _HorizotalMove = (int)_inputManager.GetAxisRaw("Horizontal");
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
        }

        private void RegisterInputAction()
        {
            _playerInput.Action_MoveAndFlip = MoveAndFlip;
            _playerInput.Action_Jump = Jump;
            _playerInput.Action_AirJump = AirJump;
            _playerInput.Action_Crouch = Crouch;
            _playerInput.Action_Stand = Stand;
            _playerInput.Action_MeleeAttack = MeleeAttack;

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
            _objectPoolManager.AddObjects_FromResourcesAsync
            (
                FrameCore.E_ResourcesPath.GameObject,
                "FX_Jump"
            );
        }

        #region BehaviorAction

        private void MoveAndFlip()
        {
            if (_HorizotalMove > 0)
            {
                this.transform.Translate(_HorizotalMove * _CurrentMoveSpeed * Time.deltaTime * Vector2.right);
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                _IsRight = true;
            }
            else if (_HorizotalMove < 0)
            {
                this.transform.Translate(_HorizotalMove * _CurrentMoveSpeed * Time.deltaTime * Vector2.left);
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                _IsRight = false;
            }
        }

        private void Jump()
        {
            _rigidbody2D.velocity = new Vector2(0f, _playerSettingInfo.controlInfo.jumpForce);
            // Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "player_jump");

            // GameObject jumpFX = Manager.GetManager<IObjectPoolManager>().GetObject("FX_Jump");
            // SetFXPos(jumpFX, _playerSettingInfo.offsetInfo.jumpFXOffset, _playerSettingInfo.offsetInfo.jumpFXOffset);
        }

        private void AirJump()
        {
            Jump();
            _playerDynamicInfo.currentJumpCount--;
        }

        private void Crouch()
        {
            _playerDynamicInfo.stateInfo.isCrouch = true;
            _playerDynamicInfo.status = E_PlayerStatus.Pistol;
            _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.crouchMoveSpeed;

            _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.crouchSize;
            _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.crouchOffset;

            // GamePanel gamePanel = GameManager.Instance.UIManager.GetExistPanel<GamePanel>();
            // if (gamePanel != null)
            //     gamePanel.UpdateAmmoPointer(_status == 0);
        }

        private void Stand()
        {
            _playerDynamicInfo.stateInfo.isCrouch = false;

            if (_playerDynamicInfo.status != E_PlayerStatus.NPC)
                _playerDynamicInfo.currentMoveSpeed = _playerSettingInfo.controlInfo.standMoveSpeed;

            _capsuleCollider2D.size = _playerSettingInfo.offsetInfo.standSize;
            _capsuleCollider2D.offset = _playerSettingInfo.offsetInfo.standOffset;
        }


        private void MeleeAttack()
        {
            if (!_playerDynamicInfo.stateInfo.isMeleeAttack)
                StartCoroutine(IE_MeleeAttack());

            StopMove();
        }

        private IEnumerator IE_MeleeAttack()
        {
            _playerDynamicInfo.stateInfo.isMeleeAttack = true;

            _animator.SetTrigger("MeleeAttack");
            // GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_meleeAttack");

            // GameObject kickFX = GameManager.Instance.ObjectPoolManager.GetObject("FX_Kick");
            // SetFXPos(kickFX, _zetriaInfo.kickFXLeftOffset, _zetriaInfo.kickFXRightOffset);

            yield return new WaitForSeconds(_playerSettingInfo.cdInfo.meleeAttackCD);
            _playerDynamicInfo.stateInfo.isMeleeAttack = false;
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







