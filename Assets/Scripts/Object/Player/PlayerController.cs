using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseCharacter
{
    private E_PlayerStatus _status;

    [Header("Move")]
    private int _horizontalMove;
    private float _standSpeed = 5;
    private float _crouchSpeed = 3;

    [Header("Jump")]
    private float _jumpForce = 15f;
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

    [Header("Attack")]
    private float _meleeAttackCD = 0.5f;
    private float _currentMeleeAttackCD;
    private float _gunAttackCD = 0.45f;
    private float _currentGunAttackCD;

    [Header("State")]
    private bool _isCrouch;
    private bool _canStand;

    [Header("Reload")]
    private float _reloadCD = 0.8f;
    private float _currentReloadCD;


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



    protected override void OnStart()
    {
        base.OnStart();

        moveSpeed = _standSpeed;
        rb2D.gravityScale = 5f;
        rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb2D.freezeRotation = true;

        _standSize = new Vector2(this.col2D.size.x, this.col2D.size.y);
        _standOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y);
        _crouchSize = new Vector2(this.col2D.size.x, this.col2D.size.y / 2);
        _crouchOffset = new Vector2(this.col2D.offset.x, this.col2D.offset.y / 2);

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (_currentMeleeAttackCD > 0)
            _currentMeleeAttackCD -= Time.deltaTime;
        if (_currentReloadCD > 0)
            _currentReloadCD -= Time.deltaTime;
        if (_currentGunAttackCD > 0)
            _currentGunAttackCD -= Time.deltaTime;

        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _currentJumpCount = _extraJumpCount;
            _canStand = CanStand();

            if (Input.GetKey(KeyCode.S) && _currentReloadCD <= 0)
            {
                Crouch();
            }
            else if (_canStand)
            {
                Stand();
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canStand && _currentReloadCD <= 0 && _currentMeleeAttackCD <= 0)
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.J) && _currentMeleeAttackCD <= 0 && !_isCrouch)
            {
                MeleeAttack();
                StopMove();
            }
            else if (Input.GetKey(KeyCode.K) && _currentGunAttackCD <= 0)
            {
                GunAttack(_status);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _status = E_PlayerStatus.Pistol;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _status = E_PlayerStatus.ShotGun;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (_currentReloadCD <= 0 && _canStand)
                {
                    Reload();
                    StopMove();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _currentJumpCount > 0)
            {
                Jump();
                _currentJumpCount--;
            }

            if (Input.GetKey(KeyCode.K) && _currentGunAttackCD <= 0)
            {
                GunAttack(_status);
            }

            Stand();
        }
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
        GameManager.Instance.AudioManager.PlayAudio(E_AudioType.Effect, "player_jump");
    }

    private void Crouch()
    {
        _isCrouch = true;
        moveSpeed = _crouchSpeed;
        col2D.size = _crouchSize;
        col2D.offset = _crouchOffset;
    }

    private void Stand()
    {
        _isCrouch = false;
        moveSpeed = _standSpeed;
        col2D.size = _standSize;
        col2D.offset = _standOffset;
    }

    private void MeleeAttack()
    {
        //TODO:设置攻击范围
        anim.SetTrigger("MeleeAttack");
        GameManager.Instance.AudioManager.PlayAudio(E_AudioType.Effect, "player_meleeAttack");
        _currentMeleeAttackCD = _meleeAttackCD;
    }

    private void GunAttack(E_PlayerStatus status)
    {
        if (_horizontalMove == 0)
            anim.SetTrigger("GunAttack");
        else if (Mathf.Abs(rb2D.velocity.y) >= 0.1f)
            anim.SetTrigger("GunAttack");

        switch (status)
        {
            case E_PlayerStatus.Pistol:
                GameManager.Instance.AudioManager.PlayAudio(E_AudioType.Effect, "pistol_fire");
                break;
            case E_PlayerStatus.ShotGun:
                break;
            case E_PlayerStatus.NPC:
                break;
        }
        _currentGunAttackCD = _gunAttackCD;
    }

    private void StopMove()
    {
        moveSpeed = 0;
        _horizontalMove = 0;
        rb2D.velocity = new Vector2(0, 0);
    }

    private void Reload()
    {
        anim.SetTrigger("Reload");
        GameManager.Instance.AudioManager.PlayAudio(E_AudioType.Effect, "pistol_reload");
        _currentReloadCD = _reloadCD;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheckPos, _groundCheckRadius);
    }

    private RaycastHit2D ShowRay(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 rayPos = this.transform.position;
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPos + offset, rayDirection, length, layer);
        Color rayColor = hitInfo ? Color.red : Color.green;
        Debug.DrawRay(rayPos + offset, rayDirection * length, rayColor);

        return hitInfo;
    }

    private bool CanStand()
    {
        _headCheck = ShowRay(RayOffset, Vector2.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
        return _headCheck ? false : true;
    }

}
