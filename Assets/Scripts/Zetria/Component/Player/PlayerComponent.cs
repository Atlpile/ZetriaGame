using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;
using System;

namespace Zetria
{
    public class PlayerComponent : BaseComponent, IDamageable
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private AudioSource _moveSource;


        private PlayerBehavior _playerBehavior;
        private PlayerInput _playerInput;
        private PlayerAnimation _playerAnimation;
        private PlayerSettingInfo _playerSettingInfo;
        [SerializeField] private PlayerDynamicInfo _playerDynamicInfo;

        private IInputManager _InputManager { get; set; }
        private IObjectPoolManager _ObjectPoolManager { get; set; }
        private IAudioManager _AudioManager { get; set; }
        private IPlayerModel _PlayerModel { get; set; }
        private IAmmoModel _AmmoModel { get; set; }

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

        private bool _IsCrouch
        {
            get => _playerDynamicInfo.stateInfo.isCrouch;
            set => _playerDynamicInfo.stateInfo.isCrouch = value;
        }

        private bool _IsReload
        {
            get => _playerDynamicInfo.stateInfo.isReload;
            set => _playerDynamicInfo.stateInfo.isReload = value;
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
            _moveSource = GetComponent<AudioSource>();

            _InputManager = Manager.GetManager<IInputManager>();
            _ObjectPoolManager = Manager.GetManager<IObjectPoolManager>();
            _AudioManager = Manager.GetManager<IAudioManager>();

            _PlayerModel = GameStructure.GetModel<IPlayerModel>();
            _AmmoModel = GameStructure.GetModel<IAmmoModel>();

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
            _playerInput = new PlayerInput(_InputManager, _playerDynamicInfo, _AmmoModel);
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

            _IsGround = UpdateGroundCheck(this.transform.position, 0.15f);
            _CanStand = UpdateStandHeadCheck(_RayOffset, 1f);

            if (!_IsReload)
                _HorizotalMove = (int)_InputManager.GetAxisRaw("Horizontal");

            if (_IsGround)
                _playerDynamicInfo.currentJumpCount = _playerSettingInfo.controlInfo.maxJumpCount;

            _moveSource.enabled = !_IsCrouch && _HorizotalMove != 0 && _IsGround;
        }

        private void FixedUpdate()
        {
            _playerInput.FixedUpdatePlayerInput();
        }

        private void OnDestroy()
        {
            UnRegisterInputAction();
            UnRegisterBehaviorAction();
        }

        /// <summary>
        /// 注册功能
        /// </summary>
        private void RegisterBehaviorAction()
        {
            _playerBehavior.OnJumpAction = OnJumpAction;
            _playerBehavior.OnMeleeAttackAction = OnMeleeAttackAction;
            // _playerBehavior.OnCrouchAction = OnCrouchAction;
            _playerBehavior.OnPistolAttackAction = OnPistolAttackAction;
            _playerBehavior.OnEmptyAttackAction = OnEmptyAttackAction;
            _playerBehavior.OnReloadAction = OnReloadAction;
            _playerBehavior.OnDamageAction = OnDamageAction;
            _playerBehavior.OnDamageCompleteAction = OnDamageCompleteAction;
            _playerBehavior.OnDeadAction = OnDeadAction;
        }

        /// <summary>
        /// 取消功能
        /// </summary>
        private void UnRegisterBehaviorAction()
        {
            //TODO：将Action放在列表中，由列表自动置空
            _playerBehavior.OnJumpAction = null;
            _playerBehavior.OnMeleeAttackAction = null;
            // _playerBehavior.OnCrouchAction = null;
            _playerBehavior.OnPistolAttackAction = null;
            _playerBehavior.OnEmptyAttackAction = null;
            _playerBehavior.OnReloadAction = null;
            _playerBehavior.OnDamageAction = null;
            _playerBehavior.OnDamageCompleteAction = null;
            _playerBehavior.OnDeadAction = null;
        }

        /// <summary>
        /// 注册Player输入
        /// </summary>
        private void RegisterInputAction()
        {
            _playerInput.Action_MoveAndFlip = _playerBehavior.MoveAndFlip;
            _playerInput.Action_Jump = _playerBehavior.Jump;
            _playerInput.Action_AirJump = _playerBehavior.AirJump;
            _playerInput.Action_Crouch = _playerBehavior.Crouch;
            _playerInput.Action_Stand = _playerBehavior.Stand;

            _playerInput.Action_MeleeAttack = MeleeAttackCoroutine;
            _playerInput.Action_PistolAttack = PistolAttackCoroutine;
            _playerInput.Action_ShortGunAttack = ShortGunAttackCoroutine;
            _playerInput.Action_EmptyAttack = EmptyAttackCoroutine;
            _playerInput.Action_Reload = ReloadCoroutine;
        }

        /// <summary>
        /// 取消Player输入
        /// </summary>
        private void UnRegisterInputAction()
        {
            //TODO：将Action放在列表中，由列表自动置空
            _playerInput.Action_MoveAndFlip = null;
            _playerInput.Action_Jump = null;
            _playerInput.Action_AirJump = null;
            _playerInput.Action_Crouch = null;
            _playerInput.Action_Stand = null;

            _playerInput.Action_MeleeAttack = null;
            _playerInput.Action_PistolAttack = null;
            _playerInput.Action_ShortGunAttack = null;
            _playerInput.Action_EmptyAttack = null;
            _playerInput.Action_Reload = null;
        }

        /// <summary>
        /// 初始化Player
        /// </summary>
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

            Manager.GetManager<IResourcesManager>().LoadAssetAsync<AudioClip>(FrameCore.E_ResourcesPath.Audio, "player_run", (audio) =>
            {
                _moveSource.clip = audio;
                //TODO:设置AudioMixer用于控制音量
            });

            _PlayerModel.CurrentHealth = _playerDynamicInfo.currentHealth;
            _PlayerModel.MaxHealth = _playerSettingInfo.maxHealth;
            _PlayerModel.IsInLevel = true;
        }

        /// <summary>
        /// 对象池预加载
        /// </summary>
        private void AddPoolObject()
        {
            _ObjectPoolManager.AddObjects_FromResourcesAsync
            (
                FrameCore.E_ResourcesPath.FX,
                "FX_Jump",
                "FX_Kick",
                "FX_Teleport"
            );

            _ObjectPoolManager.AddObjects_FromResourcesAsync
            (
                FrameCore.E_ResourcesPath.PoolObject,
                "PistolBullet"
            );
        }


        #region StartCortine

        private void MeleeAttackCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isMeleeAttack)
                StartCoroutine(_playerBehavior.IE_MeleeAttack());
        }

        private void PistolAttackCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isPistolAttack)
                StartCoroutine(_playerBehavior.IE_PistolAttack());
        }

        private void ShortGunAttackCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isShotGunAttack)
                StartCoroutine(_playerBehavior.IE_ShotGunAttack());
        }

        private void EmptyAttackCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isEmptyAttack)
                StartCoroutine(_playerBehavior.IE_EmptyAttack());
        }

        private void ReloadCoroutine()
        {
            if (!_playerDynamicInfo.stateInfo.isReload)
                StartCoroutine(_playerBehavior.IE_Reload());
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, 0.15f);
        }


        #region BehaviorAction

        private void OnJumpAction()
        {
            _AudioManager.AudioPlay(FrameCore.E_AudioType.Effect, "player_jump");
            GameObject jumpFX = _ObjectPoolManager.GetObject("FX_Jump");
            SetFXPos(
                jumpFX,
                _playerSettingInfo.offsetInfo.jumpFXOffset,
                _playerSettingInfo.offsetInfo.jumpFXOffset
            );
        }

        private void OnMeleeAttackAction()
        {
            _AudioManager.AudioPlay(FrameCore.E_AudioType.Effect, "player_meleeAttack");
            GameObject kickFX = _ObjectPoolManager.GetObject("FX_Kick");
            SetFXPos(
                kickFX,
                _playerSettingInfo.offsetInfo.kickFXLeftOffset,
                _playerSettingInfo.offsetInfo.kickFXRightOffset
            );
        }

        private void OnCrouchAction()
        {
            GameUIPanel gameUIPanel = Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>();
            gameUIPanel?.UpdateAmmoPointer(_playerDynamicInfo.status == 0);
        }

        private void OnPistolAttackAction()
        {
            _AudioManager.AudioPlay(FrameCore.E_AudioType.Effect, "pistol_fire");

            _AmmoModel.PistlAmmoInfo.currentCount--;

            var panel = Manager.GetManager<IUIManager>().GetExistPanel<GameUIPanel>();
            panel?.UpdatePistolAmmoText(_AmmoModel.PistlAmmoInfo.currentCount, _AmmoModel.PistlAmmoInfo.maxCount);

            var bullet = _ObjectPoolManager.GetObject("PistolBullet").GetComponent<PlayerBullet>();
            bullet.SetBulletTransform(_IsRight, _IsCrouch, this.transform);
        }

        private void OnShortGunAttackAction()
        {
            _AudioManager.AudioPlay(FrameCore.E_AudioType.Effect, "shotgun_fire");
            _AmmoModel.ShortGunAmmoInfo.currentCount--;

            GameObject[] bulletObjs = _ObjectPoolManager.GetObjects("ShortGunBullet", 3);
            foreach (var obj in bulletObjs)
            {
                obj.GetComponent<PlayerBullet>().SetBulletTransform(_IsRight, _IsCrouch, this.transform);
            }

            // var bulletUpward = _ObjectPoolManager.GetObject("ShortGunBullet").GetComponent<PlayerBullet>();
            // var bulletStraight = _ObjectPoolManager.GetObject("ShortGunBullet").GetComponent<PlayerBullet>();
            // var bulletDownward = _ObjectPoolManager.GetObject("ShortGunBullet").GetComponent<PlayerBullet>();

            // bulletUpward.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Upward;
            // bulletStraight.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Straight;
            // bulletDownward.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Downward;

            // SetBulletPos(
            //     bulletUpward,
            //     _playerSettingInfo.offsetInfo.shortGunBulletLeftOffset,
            //     _playerSettingInfo.offsetInfo.shortGunBulletRightOffset,
            //     _playerSettingInfo.offsetInfo.bulletOffsetWithCrouch
            // );
            // SetBulletPos(
            //     bulletStraight,
            //     _playerSettingInfo.offsetInfo.shortGunBulletLeftOffset,
            //     _playerSettingInfo.offsetInfo.shortGunBulletRightOffset,
            //     _playerSettingInfo.offsetInfo.bulletOffsetWithCrouch
            // );
            // SetBulletPos(
            //     bulletDownward,
            //     _playerSettingInfo.offsetInfo.shortGunBulletLeftOffset,
            //     _playerSettingInfo.offsetInfo.shortGunBulletRightOffset,
            //     _playerSettingInfo.offsetInfo.bulletOffsetWithCrouch
            // );


        }

        private void OnEmptyAttackAction()
        {
            _AudioManager.AudioPlay(FrameCore.E_AudioType.Effect, "gun_empty");
        }

        private void OnReloadAction()
        {
            GameStructure.SendCommand(new PlayerReloadCommand(_playerDynamicInfo.status));
        }

        private void OnDamageAction()
        {
            GameStructure.SendCommand(new PlayerDamageCommand(_playerDynamicInfo.currentHealth, _playerSettingInfo.maxHealth));
        }

        private void OnDamageCompleteAction()
        {
            Manager.GetManager<InputManager>().CanInput = true;
        }

        private void OnDeadAction()
        {
            _moveSource.enabled = false;
            GameStructure.SendCommand(new PlayerDeadCommand(
                _playerDynamicInfo.currentHealth,
                _playerSettingInfo.maxHealth
            ));
        }

        #endregion


        public void OnDamage(Vector2 attacker)
        {
            if (_playerDynamicInfo.stateInfo.isHurt && !_playerDynamicInfo.stateInfo.isDead)
                StartCoroutine(_playerBehavior.IE_Damage(attacker));
        }
    }
}







