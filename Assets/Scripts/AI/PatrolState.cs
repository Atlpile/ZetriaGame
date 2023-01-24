using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatrolState : BaseAIState
{
    public float patrolCD;
    public float currentPatrolCD;

    public PatrolState(AILogic logic) : base(logic)
    {

    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

        GroundPatrol(logic.monster.IsRight, logic.monster.MoveSpeed);
    }


    private void GroundPatrol(bool isRight, float moveSpeed)
    {
        currentPatrolCD -= Time.deltaTime;
        if (currentPatrolCD <= 0)
        {
            isRight = !isRight;
            Flip(isRight);
            currentPatrolCD = patrolCD;
        }
        Move(isRight, moveSpeed);
    }

    protected void Move(bool isRight, float moveSpeed)
    {
        if (isRight)
            logic.monster.Rb2D.velocity = Vector2.right * moveSpeed;
        else
            logic.monster.Rb2D.velocity = -Vector2.right * moveSpeed;
    }

    protected void Flip(bool isRight)
    {
        if (isRight)
            logic.monster.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        else
            logic.monster.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }
}
