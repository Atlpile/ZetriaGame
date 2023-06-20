using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseMonster : BaseCharacter
{
    [SerializeField] protected MonsterInfo monsterInfo;
    protected FSM fsm;
    protected E_AIState state = E_AIState.Null;
    protected bool isFindPlayer;
    protected bool isDead;
    protected bool canAttack;

    public bool IsFindPlayer => isFindPlayer;
    public bool IsDead => isDead;

    protected abstract void InitCharacter();
    public virtual void InitComponent() { }

    protected override void OnAwake()
    {
        base.OnAwake();

        fsm = new FSM(this);

        InitComponent();
    }

    protected override void OnStart()
    {
        base.OnStart();

        this.gameObject.layer = LayerMask.NameToLayer("Enemy");

        InitCharacter();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (fsm != null)
            fsm.UpdateFSM();
    }

    public void StopMove()
    {
        currentMoveSpeed = 0;
        rb2D.velocity = Vector3.zero;
    }

    public void UpdateMove()
    {
        if (isRight)
            rb2D.velocity = Vector2.right * currentMoveSpeed;
        else
            rb2D.velocity = -Vector2.right * currentMoveSpeed;
    }

    public void UpdateFlip()
    {
        if (isRight)
            this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        else
            this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SetMoveDirection(Transform playerPos)
    {
        if (this.transform.position.x < playerPos.position.x)
            isRight = true;
        else
            isRight = false;
    }

    public bool GetPlayer(Vector2 checkPos, float checkRadius)
    {
        if (checkRadius == 0)
            Debug.LogWarning("Enemy检测范围为0");

        return Physics2D.OverlapCircle(checkPos, checkRadius, 1 << LayerMask.NameToLayer("Player"));
    }

    public bool GetPlayer(Vector2 checkPos, Vector2 checkSize)
    {
        if (checkSize == Vector2.zero)
            Debug.LogWarning("Enemy检测范围为0");

        return Physics2D.OverlapBox(checkPos, checkSize, 0, 1 << LayerMask.NameToLayer("Player"));
    }

    protected void LossPlayer()
    {
        isFindPlayer = false;
    }

    public void ChangeSpeed(float speed)
    {
        currentMoveSpeed = speed;
    }
}
