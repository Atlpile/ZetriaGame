using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : BaseCharacter
{
    public MonsterInfo monsterInfo;
    protected AILogic aiLogic;
    protected bool isFindPlayer;
    protected bool isDead;
    protected bool canAttack;

    public bool IsDead => isDead;


    protected override void OnAwake()
    {
        base.OnAwake();

        Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (aiLogic != null)
            aiLogic.UpdateFSM();
    }

    protected abstract void Init();

    public void FSM_Idle()
    {
        isFindPlayer = false;
        currentMoveSpeed = 0;
        rb2D.velocity = Vector2.zero;
    }

    public void FSM_Chase()
    {

    }

    public void FSM_Attack()
    {

    }

    public void FSM_Dead()
    {

    }



    protected void Move(float moveSpeed)
    {
        if (isRight)
            rb2D.velocity = Vector2.right * moveSpeed;
        else
            rb2D.velocity = -Vector2.right * moveSpeed;
    }

    protected void Flip(bool isRight)
    {
        if (isRight)
            this.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        else
            this.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void ChaseMove()
    {
        isFindPlayer = true;
        currentMoveSpeed = monsterInfo.horizontalSpeed;
    }

    public void GetMoveDirection(Transform playerPos)
    {
        if (this.transform.position.x < playerPos.position.x)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
    }

    public bool GetPlayer()
    {
        return false;
    }


}
