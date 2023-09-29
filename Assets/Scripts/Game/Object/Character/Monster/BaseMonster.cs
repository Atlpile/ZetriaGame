using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseMonster : BaseCharacter, IDamageable
{
    [SerializeField] protected E_GizmosType gizmosType = E_GizmosType.Null;
    [SerializeField] protected bool canDrawGizmos = true;

    protected Transform player;
    protected SO_MonsterInfo monsterInfo;
    protected FSM fsm;
    protected float currentHealth;
    protected float destroyTime = 0.5f;
    protected float damageForce = 3f;

    protected bool isFindPlayer;
    protected bool isDead;
    protected bool isAttack = false;
    protected bool canAttack;
    [SerializeField] protected bool hasAttackForce = true;

    public SO_MonsterInfo MonsterInfo => monsterInfo;
    public bool IsFindPlayer => isFindPlayer;
    public bool IsDead => isDead;
    public bool IsAttack => isAttack;


    protected override void OnAwake()
    {
        base.OnAwake();

        fsm = new FSM(this);
        GameManager.Instance.EventManager.AddEventListener(E_EventType.PlayerDead, OnPlayerDead);

        InitComponent();
    }

    private void OnDestroy()
    {
        GameManager.Instance.EventManager.RemoveEventListener(E_EventType.PlayerDead, OnPlayerDead);
    }

    protected override void OnStart()
    {
        base.OnStart();

        this.gameObject.layer = LayerMask.NameToLayer("Enemy");

        if (GameObject.FindGameObjectWithTag("Player").transform != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        else
            Debug.LogWarning("Player为空,请检查场景中是否存在Player");

        //FIXME：创建克隆预制体时名称会不对
        // monsterInfo = GetInfo(this.name);

        InitCharacter();
        InitMoveSpeed();
        currentHealth = monsterInfo.maxHealth;
    }

    protected override void OnUpdate()
    {
        switch (gizmosType)
        {
            case E_GizmosType.Rect:
                isFindPlayer = GetPlayer(this.transform.position + monsterInfo.checkOffset, monsterInfo.checkSize);
                break;
            case E_GizmosType.Circle:
                isFindPlayer = GetPlayer(this.transform.position + monsterInfo.checkOffset, monsterInfo.checkRadius);
                break;
            case E_GizmosType.Null:
                break;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        fsm?.UpdateFSM();
    }

    protected virtual void InitComponent()
    {

    }

    public virtual void UpdateAirMove()
    {
        //怪物空中移动由怪物自己决定
    }

    public virtual void OnDamage(Vector2 attakerPos)
    {
        if (hasAttackForce)
            AddDamageForce(attakerPos);

        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "enemy_damage");

        currentHealth--;
        if (currentHealth == 0)
        {
            Dead();
        }
    }

    //OPTIMIZE:绘制检测功能可分离出来成一个脚本
    protected virtual void OnDrawGizmos()
    {
        if (canDrawGizmos && monsterInfo != null)
        {
            if (isFindPlayer)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            switch (gizmosType)
            {
                case E_GizmosType.Rect:
                    Gizmos.DrawWireCube(this.transform.position + monsterInfo.checkOffset, monsterInfo.checkSize);
                    break;
                case E_GizmosType.Circle:
                    Gizmos.DrawWireSphere(this.transform.position + monsterInfo.checkOffset, monsterInfo.checkRadius);
                    break;
                case E_GizmosType.Null:
                    break;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.OnDamage(this.transform.position);
        }
    }

    protected abstract void InitCharacter();

    public abstract void Attack();

    public abstract void Dead();


    public void StopMove()
    {
        currentMoveSpeed = 0;
        rb2D.velocity = Vector3.zero;
    }

    public void ResumeMove()
    {
        switch (monsterInfo.monsterType)
        {
            case E_MonsterType.Ground:
                currentMoveSpeed = monsterInfo.groundSpeed;
                break;
            case E_MonsterType.Fly:
                currentMoveSpeed = monsterInfo.airSpeed;
                break;
        }
    }

    public void UpdateGroundMove()
    {
        this.transform.Translate(currentMoveSpeed * Time.deltaTime * Vector2.right);
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

    protected void AddDamageForce(Vector2 attacker)
    {
        if (attacker.x < this.transform.position.x)
            rb2D.velocity = Vector2.right * damageForce;
        else
            rb2D.velocity = Vector2.left * damageForce;
    }

    protected SO_MonsterInfo GetInfo(string name)
    {
        SO_MonsterInfo info = GameManager.Instance.ResourcesLoader.Load<SO_MonsterInfo>(E_ResourcesPath.DataSO, name);
        if (info != null)
            return info;
        else
            return ScriptableObject.CreateInstance<SO_MonsterInfo>();
    }

    private void InitMoveSpeed()
    {
        if (monsterInfo.monsterType == E_MonsterType.Ground)
            currentMoveSpeed = monsterInfo.groundSpeed;
        else if (monsterInfo.monsterType == E_MonsterType.Fly)
            currentMoveSpeed = monsterInfo.airSpeed;
    }


    protected IEnumerator IE_BaseAttack()
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
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "enemy_death_02");

        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    private void OnPlayerDead()
    {
        gizmosType = E_GizmosType.Null;
        isFindPlayer = false;
        fsm.ChangeState(E_AIState.Idle);
    }

}
