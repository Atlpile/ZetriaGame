using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseCharacter, IDamageable
{

    #region Variable

    private ZetriaInfo zetriaInfo;
    private AmmoController ammoController;
    private E_PlayerStatus _status;

    //移动相关
    private int _horizontalMove;
    private AudioSource _moveSource;


    //跳跃相关
    private int _currentJumpCount;


    [Header("Player Size & Offset")]
    private Vector2 _crouchSize, _crouchOffset;
    private Vector2 _standSize, _standOffset;

    [Header("Ground Check")]
    private Vector2 _groundCheckOffset;
    private float _groundCheckRadius = 0.15f;

    [Header("Head Check")]
    private float _rayLength = 1f;
    private RaycastHit2D _headCheck;

    [Header("BulletOffset")]
    private Vector3 _pistolBulletLeftOffset = new Vector2(-1f, 1.15f);
    private Vector3 _pistolBulletRightOffset = new Vector2(1f, 1.15f);
    private Vector3 _shotGunBulletLeftOffset = new Vector2(-0.5f, 0.75f);
    private Vector3 _shotGunBulletRightOffset = new Vector2(0.5f, 0.75f);
    private Vector3 _bulletOffsetWithCrouch = new Vector2(0, -0.5f);

    [Header("FXOffset")]
    private Vector3 _kickFXRightOffset = new Vector2(0.75f, 1f);
    private Vector3 _kickFXLeftOffset = new Vector2(-0.75f, 1f);
    private Vector3 _jumpFXOffset;

    [Header("Status")]
    private bool _isMeleeAttack;
    private bool _isPistolAttack;
    private bool _isShotGunAttack;
    private bool _isEmptyAttack;
    private bool _isCrouch;
    private bool _isReload;
    private bool _isHurt;
    private bool _isDead;
    private bool _canStand;

    #endregion


    #region Attribute & Property

    public Vector2 GroundCheckPos => (Vector2)this.transform.position + _groundCheckOffset;
    public Vector2 RayOffset
    {
        get
        {
            if (isRight)
                return new Vector2(this.col2D.offset.x, this.col2D.size.y / 2);
            else
                return new Vector2(-this.col2D.offset.x, this.col2D.size.y / 2);
        }
    }
    public ZetriaInfo ZetriaInfo => zetriaInfo;
    private InputController InputController => GameManager.Instance.InputController;
    #endregion


    #region LifeCycle

    protected override void OnAwake()
    {
        base.OnAwake();

        zetriaInfo = new ZetriaInfo();
        ammoController = new AmmoController();
        _moveSource = GetComponent<AudioSource>();

        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpPistolAmmo, ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpShortGunAmmo, ammoController.PickUpShotGunAmmoPackage);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PlayerDead, OnPlayerDead);
        GameManager.Instance.EventManager.AddEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        GameManager.Instance.EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    protected override void OnStart()
    {
        base.OnStart();

        _moveSource.clip = GameManager.Instance.ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, "player_run");
        GameManager.Instance.ObjectPoolManager.AddObjectFromResources("ShortGunBullet", E_ResourcesPath.Object, 3);

        InitPlayer();

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (_isDead) return;

        _moveSource.enabled = _isCrouch || _horizontalMove == 0 || !isGround ? false : true;

        UpdatePlayerState();
    }

    protected override void OnFixedUpdate()
    {
        if (_isDead) return;

        if (!_isMeleeAttack && !_isReload)
        {
            // Move();
            // Flip();
            MoveAndFlip();
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpPistolAmmo, ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpShortGunAmmo, ammoController.PickUpShotGunAmmoPackage);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PlayerDead, OnPlayerDead);
        GameManager.Instance.EventManager.RemoveEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        GameManager.Instance.EventManager.RemoveEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);

    }

    #endregion


    #region Update & FixedUpdate

    protected override void SetAnimatorParameter()
    {
        anim.SetInteger("Horizontal", _horizontalMove);
        anim.SetFloat("Vertical", rb2D.velocity.y);
        anim.SetBool("IsGround", isGround);
        anim.SetBool("IsCrouch", _isCrouch);
        anim.SetInteger("PlayerStatus", (int)_status);
        anim.SetBool("IsDead", _isDead);

        // anim.SetBool("IsMeleeAttack", _isMeleeAttack);
    }

    private void UpdatePlayerState()
    {
        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _currentJumpCount = zetriaInfo.maxJumpCount;
            _canStand = CanStand();

            if (InputController.GetKey(E_InputType.Crouch) && !_isReload && _status != E_PlayerStatus.NPC)
                Crouch();
            else if (_canStand)
                Stand();


            if (InputController.GetKeyDown(E_InputType.Jump) && _canStand && !_isReload && !_isMeleeAttack && _status != E_PlayerStatus.NPC)
            {
                Jump();
            }
            else if (InputController.GetKeyDown(E_InputType.MeleeAttack) && !_isMeleeAttack && !_isCrouch && _status != E_PlayerStatus.NPC)
            {
                //OPTIMIZE:换子弹过程中，若攻击只能选择近战攻击或者枪械攻击的其中之一
                MeleeAttack();
                StopMove();
            }
            else if (InputController.GetKeyDown(E_InputType.SwitchWeapon) && _status != E_PlayerStatus.NPC)
            {
                ChangeWeapon();
            }
            else if (InputController.GetKeyDown(E_InputType.Reload) && CanReload() && !_isReload && !_isPistolAttack && !_isShotGunAttack && _canStand && _status != E_PlayerStatus.NPC)
            {
                Reload();
                StopMove();
            }
            else if (InputController.GetKeyDown(E_InputType.PutDownNPC) && _status == E_PlayerStatus.NPC)
            {
                PutDownNPC();
            }

            if (InputController.GetMouseButton(0) && !_isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack())
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_isShotGunAttack && _status == E_PlayerStatus.ShotGun)
            {
                if (CanAttack())
                    ShotGunAttack();
                else
                    EmptyAttack();
            }
        }
        else
        {
            if (InputController.GetKeyDown(E_InputType.Jump) && _currentJumpCount > 0 && _status != E_PlayerStatus.NPC)
            {
                Jump();
                _currentJumpCount--;
            }
            else if (InputController.GetKeyDown(E_InputType.SwitchWeapon) && _status != E_PlayerStatus.NPC && zetriaInfo.hasShortGun)
            {
                ChangeWeapon();
                //OPTIMIZE:在空中时改变切枪动画状态
            }

            if (InputController.GetMouseButton(0) && !_isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack())
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_isShotGunAttack && _status == E_PlayerStatus.ShotGun)
            {
                if (CanAttack())
                    ShotGunAttack();
                else
                    EmptyAttack();
            }

            Stand();
        }
    }

    private void MoveAndFlip()
    {
        _horizontalMove = (int)InputController.GetAxisRaw("Horizontal");
        if (_horizontalMove > 0)
        {
            this.transform.Translate(Vector2.right * _horizontalMove * currentMoveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            isRight = true;
        }
        else if (_horizontalMove < 0)
        {
            this.transform.Translate(Vector2.left * _horizontalMove * currentMoveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            isRight = false;
        }
    }

    #endregion


    #region Function

    private void InitPlayer()
    {
        zetriaInfo.hasShortGun = GameManager.Instance.SaveLoadManager.LoadData<GameData>(Consts.GameData).hasShotGun;

        isRight = true;
        currentMoveSpeed = zetriaInfo.standSpeed;
        rb2D.gravityScale = zetriaInfo.jumpGravity;
        rb2D.drag = zetriaInfo.drag;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

        _standSize = new Vector2(this.col2D.size.x, this.col2D.size.y);
        _standOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y);
        _crouchSize = new Vector2(this.col2D.size.x, this.col2D.size.y / 2);
        _crouchOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y / 2);

    }

    private void Crouch()
    {
        if (_isCrouch == false)
        {
            _isCrouch = true;
            currentMoveSpeed = zetriaInfo.crouchSpeed;
            col2D.size = _crouchSize;
            col2D.offset = _crouchOffset;
            _status = E_PlayerStatus.Pistol;

            GamePanel gamePanel = GameManager.Instance.UIManager.GetExistPanel<GamePanel>();
            if (gamePanel != null)
                gamePanel.UpdateAmmoPointer(_status == 0);
        }
    }

    private void Stand()
    {
        if (_isCrouch == true)
        {
            _isCrouch = false;
            col2D.size = _standSize;
            col2D.offset = _standOffset;

            if (_status != E_PlayerStatus.NPC)
                currentMoveSpeed = zetriaInfo.standSpeed;
        }
    }

    private void ChangeWeapon()
    {
        if (zetriaInfo.hasShortGun == true)
        {
            _status = (int)_status >= 1 ? _status = 0 : ++_status;
            GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateAmmoPointer(_status == 0);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_swapWeapon");
        }
    }

    private void PistolFire()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_fire");
        ammoController.UsePistolAmmo();

        GameObject pistolBullet = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("PistolBullet", E_ResourcesPath.Object);
        SetBulletPos(pistolBullet, _pistolBulletLeftOffset, _pistolBulletRightOffset, _bulletOffsetWithCrouch);
    }

    private void ShotGunFire()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_fire");
        ammoController.UseShotGunAmmo();

        GameObject shotGunBullet0 = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("ShortGunBullet", E_ResourcesPath.Object);
        GameObject shotGunBullet1 = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("ShortGunBullet", E_ResourcesPath.Object);
        GameObject shotGunBullet2 = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("ShortGunBullet", E_ResourcesPath.Object);

        shotGunBullet0.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Upward;
        shotGunBullet1.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Straight;
        shotGunBullet2.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Downward;

        SetBulletPos(shotGunBullet0, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
        SetBulletPos(shotGunBullet1, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
        SetBulletPos(shotGunBullet2, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
    }

    private void SetBulletPos(GameObject bullet, Vector3 leftOffset, Vector3 rightOffset, Vector3 crouchOffset)
    {
        if (isRight)
        {
            if (!_isCrouch)
                bullet.transform.position = this.transform.position + rightOffset;
            else
                bullet.transform.position = this.transform.position + rightOffset + crouchOffset;
        }
        else
        {
            if (!_isCrouch)
                bullet.transform.position = this.transform.position + leftOffset;
            else
                bullet.transform.position = this.transform.position + leftOffset + crouchOffset;
        }
        bullet.transform.localRotation = this.transform.localRotation;
    }

    private void SetFXPos(GameObject fx, Vector3 leftOffset, Vector3 rightOffset)
    {
        if (isRight)
            fx.transform.position = this.transform.position + rightOffset;
        else
            fx.transform.position = this.transform.position + leftOffset;

        fx.transform.localRotation = this.transform.localRotation;
    }

    private void StopMove()
    {
        _horizontalMove = 0;
        rb2D.velocity = Vector2.zero;
    }

    private void PutDownNPC()
    {
        _status = E_PlayerStatus.Pistol;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "npc_putdown");

        GameObject sleepWomen = GameManager.Instance.ObjectPoolManager.GetObject("SleepWomen");
        if (sleepWomen != null)
            sleepWomen.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
    }

    private void AddHurtForce(Vector2 attackerPos)
    {
        //FIXME：优化移动效果
        if (attackerPos.x < this.transform.position.x)
        {
            //向右施加弹力
            rb2D.velocity = new Vector2(2, 5);
        }
        else
        {
            //向左施加弹力
            rb2D.velocity = new Vector2(-2, 5);
        }
    }

    #endregion


    #region Coroutine Function

    private void Jump()
    {
        rb2D.velocity = new Vector2(0f, zetriaInfo.jumpForce);
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_jump");

        GameObject jumpFX = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("FX_Jump", E_ResourcesPath.FX);
        SetFXPos(jumpFX, _jumpFXOffset, _jumpFXOffset);
    }

    private void MeleeAttack()
    {
        if (!_isMeleeAttack)
            StartCoroutine(IE_MeleeAttack());
    }

    private IEnumerator IE_MeleeAttack()
    {
        _isMeleeAttack = true;

        anim.SetTrigger("MeleeAttack");
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_meleeAttack");

        GameObject kickFX = GameManager.Instance.ObjectPoolManager.GetOrLoadObject("FX_Kick", E_ResourcesPath.FX);
        SetFXPos(kickFX, _kickFXLeftOffset, _kickFXRightOffset);

        yield return new WaitForSeconds(zetriaInfo.meleeAttackCD);
        _isMeleeAttack = false;
    }

    private void PistolAttack()
    {
        if (!_isPistolAttack)
            StartCoroutine(IE_PistolAttack());
    }

    private IEnumerator IE_PistolAttack()
    {
        _isPistolAttack = true;

        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        PistolFire();

        yield return new WaitForSeconds(zetriaInfo.pistolAttackCD);
        _isPistolAttack = false;
    }

    private void ShotGunAttack()
    {
        if (!_isShotGunAttack)
            StartCoroutine(IE_ShotGunAttack());
    }

    private IEnumerator IE_ShotGunAttack()
    {
        _isShotGunAttack = true;

        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        ShotGunFire();

        yield return new WaitForSeconds(zetriaInfo.shotGunAttackCD);
        _isShotGunAttack = false;
    }

    private void EmptyAttack()
    {
        if (!_isEmptyAttack)
            StartCoroutine(IE_EmptyAttack());
    }

    private IEnumerator IE_EmptyAttack()
    {
        _isEmptyAttack = true;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "gun_empty");

        yield return new WaitForSeconds(zetriaInfo.emptyAttackCD);
        _isEmptyAttack = false;
    }

    private void Reload()
    {
        if (!_isReload)
            StartCoroutine(IE_Reload());
    }

    private IEnumerator IE_Reload()
    {
        _isReload = true;
        anim.SetTrigger("Reload");

        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                ammoController.ReloadPistolAmmo();
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_reload");
                break;
            case E_PlayerStatus.ShotGun:
                ammoController.ReloadShotGunAmmo();
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_reload");
                break;
        }

        yield return new WaitForSeconds(zetriaInfo.reloadCD);
        _isReload = false;
    }

    public void Damage(Vector2 attacker)
    {
        if (!_isHurt && !_isDead)
            StartCoroutine(IE_Damage(attacker));
    }

    private IEnumerator IE_Damage(Vector2 attacker)
    {
        _isHurt = true;

        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_hurt_1");
        GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(zetriaInfo.currentHealth, zetriaInfo.maxHealth);
        GameManager.Instance.InputController.SetInputStatus(false);

        zetriaInfo.currentHealth = zetriaInfo.currentHealth > 0 ? --zetriaInfo.currentHealth : 0;
        _isDead = zetriaInfo.currentHealth == 0 ? true : false;
        anim.SetTrigger("Hurt");
        AddHurtForce(attacker);

        if (_isDead) Dead();

        yield return new WaitForSeconds(zetriaInfo.hurtCD);
        GameManager.Instance.InputController.SetInputStatus(true);

        _isHurt = false;
    }

    private void Dead()
    {
        //TODO:死亡后，重新加载当前场景，初始化人物变量，更新UI
        _moveSource.enabled = false;
        zetriaInfo.currentHealth = 0;
        GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(zetriaInfo.currentHealth, zetriaInfo.maxHealth);
        // GameManager.Instance.EventManager.EventTrigger(E_EventType.PlayerDead);
        StopMove();


        StartCoroutine(IE_Dead());
    }

    private IEnumerator IE_Dead()
    {
        yield return new WaitForSeconds(1f);
        //TODO:UI淡入后执行以下内容

        //显示FadePanel
        GameManager.Instance.UIManager.ShowPanel<FadePanel>(true, () =>
        {
            //过渡完成后执行以下内容
            GameManager.Instance.UIManager.HidePanel<GamePanel>();
            GameManager.Instance.ClearSceneInfo();
            GameManager.Instance.SceneLoader.LoadCurrentScene();

            GameManager.Instance.UIManager.ShowPanel<GamePanel>();
            GameManager.Instance.UIManager.HidePanel<FadePanel>(true);
        });




        //TODO:UI淡出
    }

    #endregion


    #region Status

    private bool CanStand()
    {
        _headCheck = GameTools.ShowRay(this.transform.position, RayOffset, Vector2.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
        return _headCheck ? false : true;
    }

    private bool CanAttack()
    {
        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                if (ammoController.AmmoInfo.currentPistolAmmoCount != 0)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (ammoController.AmmoInfo.currentShotGunAmmoCount != 0)
                    return true;
                break;
        }

        return false;
    }

    private bool CanReload()
    {
        switch (_status)
        {
            case E_PlayerStatus.Pistol:
                if (ammoController.AmmoInfo.maxPistolAmmoCount > 0 && ammoController.AmmoInfo.currentPistolAmmoCount != ammoController.AmmoInfo.currentPistolAmmoLimit)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (ammoController.AmmoInfo.maxShotGunAmmoCount > 0 && ammoController.AmmoInfo.currentShotGunAmmoCount != ammoController.AmmoInfo.currentShotGunAmmoLimit)
                    return true;
                break;
        }
        return false;
    }

    #endregion


    #region Event

    public void OnGetNPC()
    {
        _status = E_PlayerStatus.NPC;
        currentMoveSpeed = zetriaInfo.getNPCSpeed;
    }

    public void OnGetDoorCard()
    {
        zetriaInfo.hasDoorCard = true;
    }

    public void OnPickUpShortGun()
    {
        GameData gameData = new GameData();
        gameData.hasShotGun = true;
        zetriaInfo.hasShortGun = true;

        GameManager.Instance.SaveLoadManager.SaveData(gameData, Consts.GameData);
    }

    public void OnAddHP()
    {
        zetriaInfo.currentHealth = zetriaInfo.maxHealth;
    }

    public void OnTeleportToTarget(Vector3 target)
    {
        this.transform.position = target;
    }

    public void OnUpdateAudioSourceVolume(float volume)
    {
        _moveSource.volume = volume;
    }

    public void OnPlayerDead()
    {
        _isDead = true;
        Dead();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPos, _groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseMonster monster = other.GetComponent<BaseMonster>();
        if (monster != null)
            monster.Damage(this.transform.position);
    }

    #endregion

}
