using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseMonster : BaseCharacter, IDamageable
{
    [SerializeField] protected Transform check;
    [SerializeField] protected Transform playerPos;
    [SerializeField] protected MonsterInfo monsterInfo;
    protected FSM fsm;
    protected E_AIState state = E_AIState.Null;
    protected bool isFindPlayer;
    protected bool isDead;
    protected bool isAttack = false;
    protected bool canAttack;

    public bool IsFindPlayer => isFindPlayer;
    public bool IsDead => isDead;

    protected abstract void InitCharacter();
    public virtual void InitComponent() { }

    private void Reset()
    {
        check = this.transform.GetChild(0);
    }

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

        if (GameObject.FindGameObjectWithTag("Player").transform != null)
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        InitCharacter();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (fsm != null)
            fsm.UpdateFSM();
    }

    protected override void SetAnimatorParameter()
    {
        anim.SetBool("IsFindPlayer", isFindPlayer);
        anim.SetBool("IsAttack", isAttack);
    }

    public virtual void Attack()
    {
        if (!isAttack)
            StartCoroutine(IE_BaseAttack());
    }

    private IEnumerator IE_BaseAttack()
    {
        isAttack = true;
        StopMove();
        anim.SetTrigger("Attack");
        // Debug.Log("攻击Player");
        yield return new WaitForSeconds(monsterInfo.attackDuration);

        ResumeMove();
        isAttack = false;
    }

    public void StopMove()
    {
        currentMoveSpeed = 0;
        rb2D.velocity = Vector3.zero;
    }

    public void ResumeMove()
    {
        currentMoveSpeed = monsterInfo.groundSpeed;
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

    public void FlipToPlayer()
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

    public bool CanAttack()
    {
        // Debug.Log(Vector2.Distance(this.transform.position, playerPos.transform.position));
        if (Vector2.Distance(this.transform.position, playerPos.transform.position) < monsterInfo.attackDistance)
            return true;
        else
            return false;
    }

    protected void LossPlayer()
    {

    }

    public void ChangeSpeed(float speed)
    {
        currentMoveSpeed = speed;
    }

    public void SetPatrolMove(bool isOpen)
    {
        if (isOpen)
            StartCoroutine(IE_PatrolMove());
        else
            StopCoroutine(IE_PatrolMove());
    }

    public IEnumerator IE_PatrolMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            isRight = !isRight;
        }
    }

    public virtual void Damage()
    {
        monsterInfo.currentHealth--;
        Debug.Log("Monster受伤");

        if (monsterInfo.currentHealth == 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {
        StartCoroutine(IE_BaseDead());
    }

    public IEnumerator IE_BaseDead()
    {
        anim.SetTrigger("Dead");
        StopMove();
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;

        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
