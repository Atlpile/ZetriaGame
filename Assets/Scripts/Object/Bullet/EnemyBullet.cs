using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{
    [SerializeField] private E_EnemyBulletType _type;
    [SerializeField] private Transform _playerPos;
    private float _chaseDistance = 3f;

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
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void VerticalMove()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

    private void ChaseMove()
    {
        //开始时追击Player
        this.transform.position = _playerPos.position;

        //到达一定距离时不进行追击
        if (Vector2.Distance(this.transform.position, _playerPos.position) < 3f)
        {
            //利用向量使其移动
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.Damage();
            Debug.Log("player受伤");
        }
    }

}
