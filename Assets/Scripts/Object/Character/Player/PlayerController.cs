using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseCharacter, IDamageable
{
    #region Variable

    private E_PlayerStatus _status;
    private ZetriaInfo _zetriaInfo;
    private AmmoController _ammoController;


    //移动相关
    private int _horizontalMove;
    private AudioSource _moveSource;

    [Header("Ground Check")]
    private Vector2 _groundCheckOffset;
    private float _groundCheckRadius = 0.15f;

    [Header("Head Check")]
    private RaycastHit2D _headCheck;
    private float _rayLength = 1f;

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
    public ZetriaInfo ZetriaInfo => _zetriaInfo;
    private InputController InputController => GameManager.Instance.InputController;

    #endregion


    #region LifeCycle

    protected override void OnAwake()
    {
        base.OnAwake();

        _zetriaInfo = new ZetriaInfo();
        _ammoController = new AmmoController();
        _moveSource = GetComponent<AudioSource>();

        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpPistolAmmo, _ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PickUpShortGunAmmo, _ammoController.PickUpShotGunAmmoPackage);
        GameManager.Instance.EventManager.AddEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        GameManager.Instance.EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    protected override void OnStart()
    {
        base.OnStart();

        GameManager.Instance.ResourcesLoader.LoadAsync<AudioClip>(E_ResourcesPath.Audio, "player_run", (audio) =>
        {
            _moveSource.clip = audio;
        });

        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "PistolBullet");
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.Object, "ShortGunBullet", 3);
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.FX, "FX_Jump");
        GameManager.Instance.ObjectPoolManager.AddObject(E_ResourcesPath.FX, "FX_Kick");

        InitPlayer();

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (_zetriaInfo.isDead) return;

        _moveSource.enabled = _zetriaInfo.isCrouch || _horizontalMove == 0 || !isGround ? false : true;

        UpdatePlayerState();
    }

    protected override void OnFixedUpdate()
    {
        if (_zetriaInfo.isDead) return;

        if (!_zetriaInfo.isMeleeAttack && !_zetriaInfo.isReload)
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
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpPistolAmmo, _ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PickUpShortGunAmmo, _ammoController.PickUpShotGunAmmoPackage);
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
        anim.SetBool("IsCrouch", _zetriaInfo.isCrouch);
        anim.SetInteger("PlayerStatus", (int)_status);
        anim.SetBool("IsDead", _zetriaInfo.isDead);

        // anim.SetBool("IsMeleeAttack", _isMeleeAttack);
    }

    private void UpdatePlayerState()
    {
        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _zetriaInfo.currentJumpCount = _zetriaInfo.maxJumpCount;
            _zetriaInfo.canStand = CanStand();

            if (InputController.GetKey(E_InputType.Crouch) && !_zetriaInfo.isReload && _status != E_PlayerStatus.NPC)
                Crouch();
            else if (_zetriaInfo.canStand)
                Stand();


            if (InputController.GetKeyDown(E_InputType.Jump) && _zetriaInfo.canStand && !_zetriaInfo.isReload && !_zetriaInfo.isMeleeAttack && _status != E_PlayerStatus.NPC)
            {
                Jump();
            }
            else if (InputController.GetKeyDown(E_InputType.MeleeAttack) && !_zetriaInfo.isMeleeAttack && !_zetriaInfo.isCrouch && _status != E_PlayerStatus.NPC)
            {
                //OPTIMIZE:换子弹过程中，若攻击只能选择近战攻击或者枪械攻击的其中之一
                MeleeAttack();
                StopMove();
            }
            else if (InputController.GetKeyDown(E_InputType.SwitchWeapon) && _status != E_PlayerStatus.NPC)
            {
                ChangeWeapon();
            }
            else if (
                InputController.GetKeyDown(E_InputType.Reload) &&
                CanReload() &&
                !_zetriaInfo.isReload &&
                !_zetriaInfo.isPistolAttack &&
                !_zetriaInfo.isShotGunAttack &&
                _zetriaInfo.canStand &&
                _status != E_PlayerStatus.NPC)
            {
                Reload();
                StopMove();
            }
            else if (InputController.GetKeyDown(E_InputType.PutDownNPC) && _status == E_PlayerStatus.NPC)
            {
                PutDownNPC();
            }

            if (InputController.GetMouseButton(0) && !_zetriaInfo.isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack())
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_zetriaInfo.isShotGunAttack && _status == E_PlayerStatus.ShotGun)
            {
                if (CanAttack())
                    ShotGunAttack();
                else
                    EmptyAttack();
            }
        }
        else
        {
            if (InputController.GetKeyDown(E_InputType.Jump) && _zetriaInfo.currentJumpCount > 0 && _status != E_PlayerStatus.NPC)
            {
                Jump();
                _zetriaInfo.currentJumpCount--;
            }
            else if (InputController.GetKeyDown(E_InputType.SwitchWeapon) && _status != E_PlayerStatus.NPC && _zetriaInfo.hasShortGun)
            {
                ChangeWeapon();
                //OPTIMIZE:在空中时改变切枪动画状态
            }

            if (InputController.GetMouseButton(0) && !_zetriaInfo.isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack())
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_zetriaInfo.isShotGunAttack && _status == E_PlayerStatus.ShotGun)
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
        GameData gameData = GameManager.Instance.SaveLoadManager.LoadData<GameData>(Consts.GameData);
        _zetriaInfo.hasShortGun = gameData.hasShotGun;

        _zetriaInfo.standSize = new Vector2(this.col2D.size.x, this.col2D.size.y);
        _zetriaInfo.standOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y);
        _zetriaInfo.crouchSize = new Vector2(this.col2D.size.x, this.col2D.size.y / 2);
        _zetriaInfo.crouchOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y / 2);

        isRight = true;
        currentMoveSpeed = _zetriaInfo.standMoveSpeed;

        rb2D.gravityScale = _zetriaInfo.jumpGravity;
        rb2D.drag = _zetriaInfo.drag;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    private void Crouch()
    {
        if (_zetriaInfo.isCrouch == false)
        {
            _status = E_PlayerStatus.Pistol;
            _zetriaInfo.isCrouch = true;
            currentMoveSpeed = _zetriaInfo.crouchMoveSpeed;
            col2D.size = _zetriaInfo.crouchSize;
            col2D.offset = _zetriaInfo.crouchOffset;

            GamePanel gamePanel = GameManager.Instance.UIManager.GetExistPanel<GamePanel>();
            if (gamePanel != null)
                gamePanel.UpdateAmmoPointer(_status == 0);
        }
    }

    private void Stand()
    {
        if (_zetriaInfo.isCrouch == true)
        {
            _zetriaInfo.isCrouch = false;
            col2D.size = _zetriaInfo.standSize;
            col2D.offset = _zetriaInfo.standOffset;

            if (_status != E_PlayerStatus.NPC)
                currentMoveSpeed = _zetriaInfo.standMoveSpeed;
        }
    }

    private void ChangeWeapon()
    {
        if (_zetriaInfo.hasShortGun == true)
        {
            _status = (int)_status >= 1 ? _status = 0 : ++_status;
            GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateAmmoPointer(_status == 0);
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_swapWeapon");
        }
    }

    private void PistolFire()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_fire");
        _ammoController.UsePistolAmmo();

        GameObject pistolBullet = GameManager.Instance.ObjectPoolManager.GetObject("PistolBullet");
        SetBulletPos(pistolBullet, _zetriaInfo.pistolBulletLeftOffset, _zetriaInfo.pistolBulletRightOffset, _zetriaInfo.bulletOffsetWithCrouch);
    }

    private void ShotGunFire()
    {
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_fire");
        _ammoController.UseShotGunAmmo();

        //TODO:优化
        GameObject bulletUpward = GameManager.Instance.ObjectPoolManager.GetObject("ShortGunBullet");
        GameObject bulletStraight = GameManager.Instance.ObjectPoolManager.GetObject("ShortGunBullet");
        GameObject bulletDownward = GameManager.Instance.ObjectPoolManager.GetObject("ShortGunBullet");

        bulletUpward.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Upward;
        bulletStraight.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Straight;
        bulletDownward.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Downward;

        SetBulletPos(bulletUpward, _zetriaInfo.shortGunBulletLeftOffset, _zetriaInfo.shortGunBulletRightOffset, _zetriaInfo.bulletOffsetWithCrouch);
        SetBulletPos(bulletStraight, _zetriaInfo.shortGunBulletLeftOffset, _zetriaInfo.shortGunBulletRightOffset, _zetriaInfo.bulletOffsetWithCrouch);
        SetBulletPos(bulletDownward, _zetriaInfo.shortGunBulletLeftOffset, _zetriaInfo.shortGunBulletRightOffset, _zetriaInfo.bulletOffsetWithCrouch);
    }

    private void SetBulletPos(GameObject bullet, Vector3 leftOffset, Vector3 rightOffset, Vector3 crouchOffset)
    {
        if (isRight)
        {
            if (!_zetriaInfo.isCrouch)
                bullet.transform.position = this.transform.position + rightOffset;
            else
                bullet.transform.position = this.transform.position + rightOffset + crouchOffset;
        }
        else
        {
            if (!_zetriaInfo.isCrouch)
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
        rb2D.velocity = new Vector2(0f, _zetriaInfo.jumpForce);
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_jump");

        GameObject jumpFX = GameManager.Instance.ObjectPoolManager.GetObject("FX_Jump");
        SetFXPos(jumpFX, _zetriaInfo.jumpFXOffset, _zetriaInfo.jumpFXOffset);
    }

    private void MeleeAttack()
    {
        if (!_zetriaInfo.isMeleeAttack)
            StartCoroutine(IE_MeleeAttack());
    }

    private IEnumerator IE_MeleeAttack()
    {
        _zetriaInfo.isMeleeAttack = true;

        anim.SetTrigger("MeleeAttack");
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_meleeAttack");

        GameObject kickFX = GameManager.Instance.ObjectPoolManager.GetObject("FX_Kick");
        SetFXPos(kickFX, _zetriaInfo.kickFXLeftOffset, _zetriaInfo.kickFXRightOffset);

        yield return new WaitForSeconds(_zetriaInfo.meleeAttackCD);
        _zetriaInfo.isMeleeAttack = false;
    }

    private void PistolAttack()
    {
        if (!_zetriaInfo.isPistolAttack)
            StartCoroutine(IE_PistolAttack());
    }

    private IEnumerator IE_PistolAttack()
    {
        _zetriaInfo.isPistolAttack = true;

        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        PistolFire();

        yield return new WaitForSeconds(_zetriaInfo.pistolAttackCD);
        _zetriaInfo.isPistolAttack = false;
    }

    private void ShotGunAttack()
    {
        if (!_zetriaInfo.isShotGunAttack)
            StartCoroutine(IE_ShotGunAttack());
    }

    private IEnumerator IE_ShotGunAttack()
    {
        _zetriaInfo.isShotGunAttack = true;

        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        ShotGunFire();

        yield return new WaitForSeconds(_zetriaInfo.shotGunAttackCD);
        _zetriaInfo.isShotGunAttack = false;
    }

    private void EmptyAttack()
    {
        if (!_zetriaInfo.isEmptyAttack)
            StartCoroutine(IE_EmptyAttack());
    }

    private IEnumerator IE_EmptyAttack()
    {
        _zetriaInfo.isEmptyAttack = true;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "gun_empty");

        yield return new WaitForSeconds(_zetriaInfo.emptyAttackCD);
        _zetriaInfo.isEmptyAttack = false;
    }

    private void Reload()
    {
        if (!_zetriaInfo.isReload)
            StartCoroutine(IE_Reload());
    }

    private IEnumerator IE_Reload()
    {
        _zetriaInfo.isReload = true;
        anim.SetTrigger("Reload");

        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                _ammoController.ReloadPistolAmmo();
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "pistol_reload");
                break;
            case E_PlayerStatus.ShotGun:
                _ammoController.ReloadShotGunAmmo();
                GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_reload");
                break;
        }

        yield return new WaitForSeconds(_zetriaInfo.reloadCD);
        _zetriaInfo.isReload = false;
    }

    public void Damage(Vector2 attacker)
    {
        if (!_zetriaInfo.isHurt && !_zetriaInfo.isDead)
            StartCoroutine(IE_Damage(attacker));
    }

    private IEnumerator IE_Damage(Vector2 attacker)
    {
        _zetriaInfo.isHurt = true;

        _zetriaInfo.currentHealth = _zetriaInfo.currentHealth > 0 ? --_zetriaInfo.currentHealth : 0;
        _zetriaInfo.isDead = _zetriaInfo.currentHealth == 0 ? true : false;

        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "player_hurt_1");
        GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(_zetriaInfo.currentHealth, _zetriaInfo.maxHealth);
        GameManager.Instance.InputController.SetInputStatus(false);

        anim.SetTrigger("Hurt");
        AddHurtForce(attacker);
        if (_zetriaInfo.isDead) Dead();

        yield return new WaitForSeconds(_zetriaInfo.hurtCD);
        GameManager.Instance.InputController.SetInputStatus(true);

        _zetriaInfo.isHurt = false;
    }

    private void Dead()
    {
        _moveSource.enabled = false;
        _zetriaInfo.currentHealth = 0;

        GameManager.Instance.EventManager.EventTrigger(E_EventType.PlayerDead);
        GameManager.Instance.UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(_zetriaInfo.currentHealth, _zetriaInfo.maxHealth);
        StopMove();

        GameManager.Instance.LoadCurrentScene();
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
                if (_ammoController.AmmoInfo.currentPistolAmmoCount != 0)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (_ammoController.AmmoInfo.currentShotGunAmmoCount != 0)
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
                if (_ammoController.AmmoInfo.maxPistolAmmoCount > 0 && _ammoController.AmmoInfo.currentPistolAmmoCount != _ammoController.AmmoInfo.currentPistolAmmoLimit)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (_ammoController.AmmoInfo.maxShotGunAmmoCount > 0 && _ammoController.AmmoInfo.currentShotGunAmmoCount != _ammoController.AmmoInfo.currentShotGunAmmoLimit)
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
        currentMoveSpeed = _zetriaInfo.getNPCMoveSpeed;
    }

    public void OnGetDoorCard()
    {
        _zetriaInfo.hasDoorCard = true;
    }

    public void OnPickUpShortGun()
    {
        GameData gameData = new GameData();
        gameData.hasShotGun = true;

        _zetriaInfo.hasShortGun = true;

        GameManager.Instance.SaveLoadManager.SaveData(gameData, Consts.GameData);
    }

    public void OnAddHP()
    {
        _zetriaInfo.currentHealth = _zetriaInfo.maxHealth;
    }

    public void OnTeleportToTarget(Vector3 target)
    {
        this.transform.position = target;
    }

    public void OnUpdateAudioSourceVolume(float volume)
    {
        _moveSource.volume = volume;
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
