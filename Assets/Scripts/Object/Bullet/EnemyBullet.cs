using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{
    public E_EnemyBulletType type;
    [SerializeField] private Transform _playerPos;
    private float _chaseDistance = 3f;

    protected override void InitBullet()
    {
        moveSpeed = 10f;
        currentMoveSpeed = moveSpeed;

        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Create()
    {
        base.Create();

        anim.Play("Run");
    }

    private void Update()
    {
        Move(type);
    }



    private void Move(E_EnemyBulletType type)
    {
        switch (type)
        {
            case E_EnemyBulletType.Horizontal:
                HorizontalMove();
                break;
            case E_EnemyBulletType.Vertical:
                VerticalMove();
                break;
            case E_EnemyBulletType.Chase:
                ChaseMove();
                break;
        }
    }

    private void HorizontalMove()
    {
        transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
    }

    private void VerticalMove()
    {
        transform.Translate(Vector2.down * currentMoveSpeed * Time.deltaTime);
    }

    private void ChaseMove()
    {
        //开始时追击Player
        this.transform.position = _playerPos.position;

        //到达一定距离时不进行追击
        if (Vector2.Distance(this.transform.position, _playerPos.position) < _chaseDistance)
        {
            //利用向量使其移动
        }
    }


}
