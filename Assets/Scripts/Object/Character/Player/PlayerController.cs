using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacter
{
    private E_PlayerStatus _status;

    [Header("Move")]
    private int _horizontalMove;
    private float _standSpeed = 4f;
    private float _getNPCSpeed = 3.5f;
    private float _crouchSpeed = 2f;
    private AudioSource _moveSource;

    [Header("Jump")]
    private float _jumpForce = 12f;
    private int _extraJumpCount = 1;
    private int _currentJumpCount;

    [Header("Crouch & Stand")]
    private Vector2 _crouchSize;
    private Vector2 _crouchOffset;
    private Vector2 _standSize;
    private Vector2 _standOffset;

    [Header("GroundCheck")]
    private Vector2 _groundCheckPos;
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

    [Header("Attack")]
    private float _meleeAttackCD = 0.4f;
    private float _currentMeleeAttackCD;
    private float _pistolAttackCD = 0.45f;
    private float _currentpistolAttackCD;
    private float _shotGunAttackCD = 0.9f;
    private float _currentShotGunAttackCD;

    [Header("State")]
    private bool _isCrouch;
    private bool _canStand;

    [Header("Reload")]
    private float _reloadCD = 0.8f;
    private float _currentReloadCD;

    [Header("Hurt")]
    private float _hurtCD = 0.4f;
    private float _currentHurtCD;


    public Vector2 GroundCheckPos => (Vector2)this.transform.position + _groundCheckPos;
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


    protected override void OnAwake()
    {
        base.OnAwake();
        _moveSource = GetComponent<AudioSource>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        isRight = true;
        _moveSource.clip = GameManager.Instance.m_ResourcesLoader.Load<AudioClip>(E_ResourcesPath.Audio, "player_run");

        moveSpeed = _standSpeed;
        rb2D.gravityScale = 5f;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

        _standSize = new Vector2(this.col2D.size.x, this.col2D.size.y);
        _standOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y);
        _crouchSize = new Vector2(this.col2D.size.x, this.col2D.size.y / 2);
        _crouchOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y / 2);

        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpNPC, OnGetNPC);
        GameManager.Instance.m_EventManager.AddEventListener(E_EventType.PickUpShotGun, () => { });

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        _moveSource.enabled = _isCrouch || _horizontalMove == 0 || !isGround ? false : true;

        if (_currentMeleeAttackCD > 0)
            _currentMeleeAttackCD -= Time.deltaTime;
        if (_currentReloadCD > 0)
            _currentReloadCD -= Time.deltaTime;
        if (_currentpistolAttackCD > 0)
            _currentpistolAttackCD -= Time.deltaTime;
        if (_currentHurtCD > 0)
            _currentHurtCD -= Time.deltaTime;
        if (_currentShotGunAttackCD > 0)
            _currentShotGunAttackCD -= Time.deltaTime;

        UpdatePlayerState();
    }

    protected override void OnFixedUpdate()
    {
        if (_currentMeleeAttackCD <= 0 && _currentReloadCD <= 0)
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
    }


    private void UpdatePlayerState()
    {
        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _currentJumpCount = _extraJumpCount;
            _canStand = CanStand();

            if (Input.GetKey(KeyCode.S) && _currentReloadCD <= 0 && _status != E_PlayerStatus.NPC)
            {
                Crouch();
            }
            else if (_canStand)
            {
                Stand();
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canStand && _currentReloadCD <= 0 && _currentMeleeAttackCD <= 0 && _status != E_PlayerStatus.NPC)
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.F) && _currentMeleeAttackCD <= 0 && !_isCrouch && _status != E_PlayerStatus.NPC)
            {
                //OPTIMIZE:换子弹过程中，若攻击只能选择近战攻击或者枪械攻击的其中之一
                MeleeAttack();
                StopMove();
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && _status != E_PlayerStatus.NPC)
            {
                AlterWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.R) && _currentReloadCD <= 0 && _currentpistolAttackCD <= 0 && _currentShotGunAttackCD <= 0 && _canStand && GameManager.Instance.m_AmmoManager.CanReload(_status) && _status != E_PlayerStatus.NPC)
            {
                Reload();
                StopMove();
            }
            else if (Input.GetKeyDown(KeyCode.Q) && _status == E_PlayerStatus.NPC)
            {
                PutDownNPC();
            }
            else if (Input.GetKeyDown(KeyCode.H) && _currentHurtCD <= 0)
            {
                Hurt();
            }

            if (Input.GetMouseButton(0) && _currentpistolAttackCD <= 0 && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (GameManager.Instance.m_AmmoManager.CanAttack(_status))
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (Input.GetMouseButton(0) && _currentShotGunAttackCD <= 0 && _status == E_PlayerStatus.ShotGun)
            {
                if (GameManager.Instance.m_AmmoManager.CanAttack(_status))
                    ShotGunAttack();
                else
                    EmptyAttack();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _currentJumpCount > 0 && _status != E_PlayerStatus.NPC)
            {
                Jump();
                _currentJumpCount--;
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && _status != E_PlayerStatus.NPC)
            {
                AlterWeapon();
                //OPTIMIZE:在空中时改变切枪动画状态
            }

            if (Input.GetMouseButton(0) && _currentpistolAttackCD <= 0 && (_status == E_PlayerStatus.Pistol || _status == E_PlayerStatus.NPC))
            {
                if (GameManager.Instance.m_AmmoManager.CanAttack(_status))
                    PistolAttack();
                else
                    EmptyAttack();
            }
            if (Input.GetMouseButton(0) && _currentShotGunAttackCD <= 0 && _status == E_PlayerStatus.ShotGun)
            {
                if (GameManager.Instance.m_AmmoManager.CanAttack(_status))
                    ShotGunAttack();
                else
                    EmptyAttack();
            }

            Stand();
        }
    }

    private void Move()
    {
        _horizontalMove = (int)Input.GetAxisRaw("Horizontal");
        rb2D.velocity = new Vector2(moveSpeed * _horizontalMove, rb2D.velocity.y);
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
        rb2D.velocity = new Vector2(0f, _jumpForce);
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "player_jump");
    }

    private void Crouch()
    {
        _isCrouch = true;
        moveSpeed = _crouchSpeed;
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
            moveSpeed = _standSpeed;
    }

    private void MeleeAttack()
    {
        //TODO:设置攻击范围
        anim.SetTrigger("MeleeAttack");
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "player_meleeAttack");
        _currentMeleeAttackCD = _meleeAttackCD;
    }

    private void PistolAttack()
    {
        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        PistolFire();
        _currentpistolAttackCD = _pistolAttackCD;
    }

    private void ShotGunAttack()
    {
        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        ShotGunFire();
        _currentShotGunAttackCD = _shotGunAttackCD;
    }

    private void EmptyAttack()
    {
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "gun_empty");

        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                _currentpistolAttackCD = _pistolAttackCD;
                break;
            case E_PlayerStatus.ShotGun:
                _currentShotGunAttackCD = _shotGunAttackCD;
                break;
        }
    }

    private void AlterWeapon()
    {
        _status = (int)_status >= 1 ? _status = 0 : ++_status;
        GameManager.Instance.m_UIManager.GetExistPanel<GamePanel>().UpdateAmmoPointer(_status == 0);
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "player_swapWeapon");
    }

    private void PistolFire()
    {
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "pistol_fire");
        GameManager.Instance.m_AmmoManager.UsePistolAmmo();

        GameObject pistolBullet = GameManager.Instance.m_ObjectPool.GetOrLoadObject("PistolBullet", E_ResourcesPath.Entity);
        AlterBulletPos(pistolBullet, _pistolBulletLeftOffset, _pistolBulletRightOffset, _bulletOffsetWithCrouch);
    }

    private void ShotGunFire()
    {
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "shotgun_fire");
        GameManager.Instance.m_AmmoManager.UseShotGunAmmo();

        GameObject shotGunBullet0 = GameManager.Instance.m_ObjectPool.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);
        GameObject shotGunBullet1 = GameManager.Instance.m_ObjectPool.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);
        GameObject shotGunBullet2 = GameManager.Instance.m_ObjectPool.GetOrLoadObject("ShotGunBullet", E_ResourcesPath.Entity);

        shotGunBullet0.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Upward;
        shotGunBullet2.GetComponent<ShotGunBullet>().moveType = E_BulletMoveType.Downward;

        AlterBulletPos(shotGunBullet0, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
        AlterBulletPos(shotGunBullet1, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
        AlterBulletPos(shotGunBullet2, _shotGunBulletLeftOffset, _shotGunBulletRightOffset, _bulletOffsetWithCrouch);
    }

    private void AlterBulletPos(GameObject bullet, Vector3 leftOffset, Vector3 rightOffset, Vector3 crouchOffset)
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

    private void Reload()
    {
        switch (_status)
        {
            case E_PlayerStatus.NPC:
            case E_PlayerStatus.Pistol:
                GameManager.Instance.m_AmmoManager.ReloadPistolAmmo();
                GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "pistol_reload");
                break;
            case E_PlayerStatus.ShotGun:
                GameManager.Instance.m_AmmoManager.ReloadShotGunAmmo();
                GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "shotgun_reload");
                break;
        }

        anim.SetTrigger("Reload");
        _currentReloadCD = _reloadCD;

    }

    private void Hurt()
    {
        anim.SetTrigger("Hurt");
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "player_hurt_1");
        _currentHurtCD = _hurtCD;
    }

    private void PutDownNPC()
    {
        _status = E_PlayerStatus.Pistol;
        GameManager.Instance.m_AudioManager.PlayAudio(E_AudioType.Effect, "npc_putdown");

        GameObject sleepWomen = GameManager.Instance.m_ObjectPool.GetPoolObject("SleepWomen");
        sleepWomen.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
    }

    public void OnGetNPC()
    {
        _status = E_PlayerStatus.NPC;
        moveSpeed = _getNPCSpeed;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPos, _groundCheckRadius);
    }

    private bool CanStand()
    {
        _headCheck = GameTools.ShowRay(this.transform.position, RayOffset, Vector2.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
        return _headCheck ? false : true;
    }

}
