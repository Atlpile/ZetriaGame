using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : BaseCharacter
{
    protected AILogic aiLogic;

    protected override void OnAwake()
    {
        base.OnAwake();

        Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (aiLogic != null)
            aiLogic.UpdateState();
    }

    public abstract void Init();


    // private void GroundPatrol(bool isRight, float moveSpeed)
    // {
    //     currentPatrolCD -= Time.deltaTime;
    //     if (currentPatrolCD <= 0)
    //     {
    //         isRight = !isRight;
    //         Flip(isRight);
    //         currentPatrolCD = patrolCD;
    //     }
    //     Move(isRight, moveSpeed);
    // }

    // protected void Move(bool isRight, float moveSpeed)
    // {
    //     if (isRight)
    //         logic.monster.Rb2D.velocity = Vector2.right * moveSpeed;
    //     else
    //         logic.monster.Rb2D.velocity = -Vector2.right * moveSpeed;
    // }

    // protected void Flip(bool isRight)
    // {
    //     if (isRight)
    //         logic.monster.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    //     else
    //         logic.monster.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    // }
}
