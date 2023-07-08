using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseMonster : BaseCharacter, IDamageable
{
    [SerializeField] protected Transform check;
    [SerializeField] protected Transform player;
    [SerializeField] protected MonsterInfo monsterInfo;
    protected FSM fsm;
    protected E_AIState state = E_AIState.Null;
    protected float destroyTime = 0.5f;
    protected bool isFindPlayer;
    protected bool isDead;
    protected bool isAttack = false;
    protected bool canAttack;

    public MonsterInfo MonsterInfo => monsterInfo;
    public bool IsFindPlayer => isFindPlayer;
    public bool IsDead => isDead;
    public bool IsAttack => isAttack;


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
            player = GameObject.FindGameObjectWithTag("Player").transform;

        InitCharacter();
    }

    protected override void OnUpdate()
    {
        //TODO:IsFindPlayer重复优化
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (fsm != null)
            fsm.UpdateFSM();
    }

    public virtual void InitComponent()
    {
        check = this.transform.GetChild(0);
    }

    protected abstract void InitCharacter();

    public abstract void Attack();

    public abstract void Dead();

    public virtual void Damage(Vector2 attakerPos)
    {
        AddDamageForce(attakerPos);
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "enemy_damage");

        monsterInfo.currentHealth--;
        if (monsterInfo.currentHealth == 0)
        {
            Dead();
        }
    }


    public void StopMove()
    {
        currentMoveSpeed = 0;
        rb2D.velocity = Vector3.zero;
    }

    public void ResumeMove()
    {
        currentMoveSpeed = monsterInfo.groundSpeed;
        currentMoveSpeed = monsterInfo.airSpeed;
    }

    public void UpdateGroundMove()
    {
        this.transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
    }

    public virtual void UpdateAirMove()
    {
        //怪物空中移动由怪物自己决定
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
        if (this.transform.position.x < player.position.x)
            isRight = true;
        else
            isRight = false;
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
        if (Vector2.Distance(this.transform.position, player.transform.position) < monsterInfo.attackDistance)
            return true;
        else
            return false;
    }

    protected void LossPlayer()
    {

    }

    protected void AddDamageForce(Vector2 attacker)
    {
        if (attacker.x < this.transform.position.x)
        {
            Debug.Log("向右施加力");
            rb2D.velocity = Vector2.right * 3;
        }
        else
        {
            Debug.Log("向左施加力");
            rb2D.velocity = Vector2.left * 3;
        }
    }



    private IEnumerator IE_PatrolMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            isRight = !isRight;
        }
    }

    private IEnumerator IE_BaseAttack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        StopMove();

        yield return new WaitForSeconds(monsterInfo.attackDuration);
        ResumeMove();
        isAttack = false;
    }

    protected IEnumerator IE_BaseDead()
    {
        anim.SetTrigger("Dead");
        StopMove();
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;
        GameManager.Instance.m_AudioManager.AudioPlay(E_AudioType.Effect, "enemy_death_02");

        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
