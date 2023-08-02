using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : BaseBullet
{
    protected override void InitBullet()
    {
        moveSpeed = 10f;
        currentMoveSpeed = moveSpeed;
    }

    protected override void Create()
    {
        base.Create();
        anim.Play("Run");
    }



    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
    }


}
