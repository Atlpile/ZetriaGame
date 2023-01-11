using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : BaseCharacter
{
    private E_PlayerStatus _playerStatus;

    [Header("Move")]
    private int _horizontalMove;
    private float _standSpeed = 5;
    private float _crouchSpeed = 3;

    [Header("Jump")]
    private float _jumpForce = 15f;
    private int _currentJumpCount;
    private int _extraJumpCount = 1;

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

    [Header("State")]
    private bool _isCrouch;
    private bool _canStand;


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

        isGround = GetGround(GroundCheckPos, _groundCheckRadius);
        if (isGround)
        {
            _currentJumpCount = _extraJumpCount;
            _canStand = CanStand();

            if (Input.GetKey(KeyCode.S))
            {
                Crouch();
            }
            else if (_canStand)
            {
                Stand();
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canStand)
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                if (_currentMeleeAttackCD <= 0 && !_isCrouch)
                {
                    MeleeAttack();
                    StopMove();
                }
            }
            // else if (Input.GetKeyDown(KeyCode.K))
            // {
            //     anim.SetTrigger("GunAttack");
            //     GunAttack();
            // }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _playerStatus = E_PlayerStatus.Pistol;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _playerStatus = E_PlayerStatus.ShotGun;
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _currentJumpCount > 0)
            {
                Jump();
                _currentJumpCount--;
            }
            Stand();
        }
    }

    protected override void OnFixedUpdate()
    {
        if (_currentMeleeAttackCD <= 0)
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
        anim.SetInteger("PlayerStatus", (int)_playerStatus);
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
        _currentMeleeAttackCD = _meleeAttackCD;
    }

    private void StopMove()
    {
        moveSpeed = 0;
        _horizontalMove = 0;
        rb2D.velocity = new Vector2(0, 0);
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
