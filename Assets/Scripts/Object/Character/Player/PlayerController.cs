using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseCharacter
{
    private ZetriaInfo zetriaInfo;
    private AmmoController ammoController;
    private E_PlayerStatus _status;

    //移动相关
    private int _horizontalMove;
    private AudioSource _moveSource;


    //跳跃相关
    private int _currentJumpCount;
    private Vector3 _jumpFXOffset;

    [Header("Player Size & Offset")]
    private Vector2 _crouchSize;
    private Vector2 _crouchOffset;
    private Vector2 _standSize;
    private Vector2 _standOffset;

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
    private InputController InputController => GameManager.Instance.m_InputController;

    protected override void OnAwake()
    {
        base.OnAwake();

        zetriaInfo = new ZetriaInfo();
        ammoController = new AmmoController();
        _moveSource = GetComponent<AudioSource>();

        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpPistolAmmo, ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShortGunAmmo, ammoController.PickUpShotGunAmmoPackage);
        GameManager.Instance.m_EventManager.AddEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        GameManager.Instance.m_EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);
    }

    protected override void OnStart()
    {
        base.OnStart();

        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpNPC, OnGetNPC);
        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpPistolAmmo, ammoController.PickUpPistolAmmoPackage);
        // GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShortGunAmmo, ammoController.PickUpShotGunAmmoPackage);
        // GameManager.Instance.m_EventManager.AddEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        // GameManager.Instance.m_EventManager.AddEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);

        _moveSource.clip = GameManager.Instance.m_ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, "player_run");
        GameManager.Instance.m_ObjectPoolManager.AddObjectFromResources("ShotGunBullet", E_ResourcesPath.Entity, 3);

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
            Move();
            Flip();
        }
    }

    protected override void SetAnimatorParameter()
    {
        anim.SetInteger("Horizontal", _horizontalMove);
        anim.SetFloat("Vertical", rb2D.velocity.y);
        anim.SetBool("IsGround", isGround);
        anim.SetBool("IsCrouch", _isCrouch);
        anim.SetInteger("PlayerStatus", (int)_status);
        anim.SetBool("IsDead", _isDead);
    }

    private void OnDestroy()
    {
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpShortGun, OnPickUpShortGun);
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpDoorCard, OnGetDoorCard);
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpPistolAmmo, ammoController.PickUpPistolAmmoPackage);
        GameManager.Instance.m_EventManager.RemoveEventListener(E_EventType.PickUpShortGunAmmo, ammoController.PickUpShotGunAmmoPackage);
        GameManager.Instance.m_EventManager.RemoveEventListener<Vector3>(E_EventType.PlayerTeleport, OnTeleportToTarget);
        GameManager.Instance.m_EventManager.RemoveEventListener<float>(E_EventType.UpdateAudioSourceVolume, OnUpdateAudioSourceVolume);

    }

    private void InitPlayer()
    {
        isRight = true;
        currentMoveSpeed = zetriaInfo.standSpeed;
        rb2D.gravityScale = zetriaInfo.jumpGravity;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

        _standSize = new Vector2(this.col2D.size.x, this.col2D.size.y);
        _standOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y);
        _crouchSize = new Vector2(this.col2D.size.x, this.col2D.size.y / 2);
        _crouchOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y / 2);
    }

    private void UpdatePlayerState()
    {
        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _currentJumpCount = zetriaInfo.maxJumpCount;
            _canStand = CanStand();

            if (InputController.GetKey(E_InputType.Crouch) && !_isReload && _status != E_PlayerStatus.NPC)
            {
                Crouch();
            }
            else if (_canStand)
            {
                Stand();
            }

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
                AlterWeapon();
            }
            else if (InputController.GetKeyDown(E_InputType.Reload) && CanReload(_status) && !_isReload && !_isPistolAttack && !_isShotGunAttack && _canStand && _status != E_PlayerStatus.NPC)
            {
                Reload();
                StopMove();
            }
            else if (InputController.GetKeyDown(E_InputType.PutDownNPC) && _status == E_PlayerStatus.NPC)
            {
                PutDownNPC();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Hurt();
            }

            if (InputController.GetMouseButton(0) && !_isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack(_status))
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_isShotGunAttack && _status == E_PlayerStatus.ShotGun)
            {
                if (CanAttack(_status))
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
            else if (InputController.GetKeyDown(E_InputType.SwitchWeapon) && _status != E_PlayerStatus.NPC)
            {
                AlterWeapon();
                //OPTIMIZE:在空中时改变切枪动画状态
            }

            if (InputController.GetMouseButton(0) && !_isPistolAttack && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (CanAttack(_status))
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (InputController.GetMouseButton(0) && !_isShotGunAttack && _status == E_PlayerStatus.ShotGun)
            {
                if (CanAttack(_status))
                    ShotGunAttack();
                else
                    EmptyAttack();
            }

            Stand();
        }
    }

    private void Move()
    {
        _horizontalMove = (int)InputController.GetAxisRaw("Horizontal");
        rb2D.velocity = new Vector2(currentMoveSpeed * _horizontalMove, rb2D.velocity.y);
    }

    private void Flip()
    {
        if (_horizontalMove > 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            isRight = true;
        }
        else if (_horizontalMove < 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            isRight = false;
        }
    }

    private void Jump()
    {
        rb2D.velocity = new Vector2(0f, zetriaInfo.jumpForce);
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "player_jump");

        // GameObject jumpFX = GameManager.Instance.m_ObjectPool.GetOrLoadObject("fx_jump", E_ResourcesPath.FX);
        // jumpFX.transform.position = this.transform.position + _jumpFXOffset;

        //TODO:创建的FX，通过协程延时调用返回至对象池
    }

    private IEnumerator IE_Jump()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void Crouch()
    {
        _isCrouch = true;
        currentMoveSpeed = zetriaInfo.crouchSpeed;
        col2D.size = _crouchSize;
        col2D.offset = _crouchOffset;
        _status = E_PlayerStatus.Pistol;
        GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>().UpdateAmmoPointer(_status == 0);
    }

    private void Stand()
    {
        _isCrouch = false;
        col2D.size = _standSize;
        col2D.offset = _standOffset;

        if (_status != E_PlayerStatus.NPC)
            currentMoveSpeed = zetriaInfo.standSpeed;
    }

    private void AlterWeapon()
    {
        _status = (int)_status >= 1 ? _status = 0 : ++_status;
        GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>().UpdateAmmoPointer(_status == 0);
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "player_swapWeapon");
    }

    private void PistolFire()
    {
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "pistol_fire");
        ammoController.UsePistolAmmo();

        GameObject pistolBullet = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("PistolBullet", E_ResourcesPath.Entity);
        SetBulletPos(pistolBullet, _pistolBulletLeftOffset, _pistolBulletRightOffset, _bulletOffsetWithCrouch);
    }

    private void ShotGunFire()
    {
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_fire");
        ammoController.UseShotGunAmmo();

        GameObject shotGunBullet0 = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);
        GameObject shotGunBullet1 = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);
        GameObject shotGunBullet2 = GameManager.Instance.m_ObjectPoolManager.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);

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

    private void StopMove()
    {
        _horizontalMove = 0;
        rb2D.velocity = new Vector2(0, 0);
    }


    private void MeleeAttack()
    {
        if (!_isMeleeAttack)
            StartCoroutine(IE_MeleeAttack());
    }

    private IEnumerator IE_MeleeAttack()
    {
        _isMeleeAttack = true;

        //TODO:设置攻击范围
        anim.SetTrigger("MeleeAttack");
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "player_meleeAttack");

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
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "gun_empty");

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
                GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "pistol_reload");
                break;
            case E_PlayerStatus.ShotGun:
                ammoController.ReloadShotGunAmmo();
                GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "shotgun_reload");
                break;
        }

        yield return new WaitForSeconds(zetriaInfo.reloadCD);
        _isReload = false;
    }

    public void Hurt()
    {
        if (!_isHurt && !_isDead)
            StartCoroutine(IE_Hurt());
    }

    private IEnumerator IE_Hurt()
    {
        _isHurt = true;

        zetriaInfo.currentHealth = zetriaInfo.currentHealth > 0 ? --zetriaInfo.currentHealth : 0;
        _isDead = zetriaInfo.currentHealth == 0 ? true : false;

        anim.SetTrigger("Hurt");

        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "player_hurt_1");
        GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>().UpdateLifeBar(zetriaInfo.currentHealth, zetriaInfo.maxHealth);

        if (_isDead) Dead();

        yield return new WaitForSeconds(zetriaInfo.hurtCD);
        _isHurt = false;
    }

    private void Dead()
    {
        //TODO:死亡后，重新加载当前场景，初始化人物变量，更新UI
        print("死亡时做的事情");
        _moveSource.enabled = false;
        StopMove();
    }

    private void PutDownNPC()
    {
        _status = E_PlayerStatus.Pistol;
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "npc_putdown");

        GameObject sleepWomen = GameManager.Instance.m_ObjectPoolManager.GetObject("SleepWomen");
        if (sleepWomen != null)
            sleepWomen.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
    }

    private bool CanStand()
    {
        _headCheck = GameTools.ShowRay(this.transform.position, RayOffset, Vector2.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
        return _headCheck ? false : true;
    }

    private bool CanAttack(E_PlayerStatus status)
    {
        switch (status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                if (ammoController.AmmoInfo._currentPistolAmmoCount != 0)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (ammoController.AmmoInfo._currentShotGunAmmoCount != 0)
                    return true;
                break;
        }

        return false;
    }

    private bool CanReload(E_PlayerStatus status)
    {
        switch (status)
        {
            case E_PlayerStatus.Pistol:
                if (ammoController.AmmoInfo._maxPistolAmmoCount > 0 && ammoController.AmmoInfo._currentPistolAmmoCount != ammoController.AmmoInfo._currentPistolAmmoLimit)
                    return true;
                break;
            case E_PlayerStatus.ShotGun:
                if (ammoController.AmmoInfo._maxShotGunAmmoCount > 0 && ammoController.AmmoInfo._currentShotGunAmmoCount != ammoController.AmmoInfo._currentShotGunAmmoLimit)
                    return true;
                break;
        }
        return false;
    }


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
        //TODO：拾取到霰弹枪时，才可以使用霰弹枪
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPos, _groundCheckRadius);
    }

    #endregion
}
