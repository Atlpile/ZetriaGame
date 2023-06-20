using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class BaseCharacter : MonoBehaviour
{
    protected Animator anim;
    protected CapsuleCollider2D col2D;
    protected Rigidbody2D rb2D;
    protected float currentMoveSpeed;
    protected bool isRight;
    protected bool isGround;

    private void Awake()
    {
        col2D = this.GetComponent<CapsuleCollider2D>();
        rb2D = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();

        OnAwake();
    }

    private void Start()
    {
        rb2D.freezeRotation = true;

        OnStart();
    }

    private void Update()
    {
        OnUpdate();
        SetAnimatorParameter();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }


    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

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

    public void PlayAnim(string animName)
    {
        anim.Play(animName);
    }
}
