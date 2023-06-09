using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : BaseObject
{
    protected CapsuleCollider2D col2D;
    protected Rigidbody2D rb2D;
    protected float moveSpeed;
    protected bool isRight;
    protected bool isGround;


    private void Update()
    {
        OnUpdate();
        SetAnimatorParameter();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }


    protected override void OnAwake()
    {
        base.OnAwake();

        col2D = this.GetComponent<CapsuleCollider2D>();
        rb2D = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        rb2D.freezeRotation = true;
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnFixedUpdate()
    {

    }



    protected virtual void SetAnimatorParameter()
    {

    }

    protected bool GetGround(Vector2 groundCheckPos, float checkRadius)
    {
        if (checkRadius == 0)
            Debug.LogWarning("当前地面检测范围为0");

        if (rb2D.velocity.y <= 0.1f)
            return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 1 << LayerMask.NameToLayer("Ground"));

        return Physics2D.OverlapCircle(groundCheckPos, checkRadius, 0);
    }
}
