using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBullet : MonoBehaviour
{
    public E_BulletMoveType moveType;
    public float verticalSpeed;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _destroyTime = 0.5f;

    private void OnEnable()
    {
        Create();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (moveType)
        {
            case E_BulletMoveType.Upward:
                transform.Translate(new Vector2(1, 0 + verticalSpeed) * Time.deltaTime * _moveSpeed);
                break;
            case E_BulletMoveType.Straight:
                transform.Translate(Vector2.right * Time.deltaTime * _moveSpeed);
                break;
            case E_BulletMoveType.Downward:
                transform.Translate(new Vector2(1, 0 - verticalSpeed) * Time.deltaTime * _moveSpeed);
                break;
        }

    }

    private Vector2 SlowMove()
    {
        return Vector2.zero * Time.deltaTime;
    }

    public void Create()
    {
        Invoke("Hide", _destroyTime);
    }

    public void Hide()
    {
        GameManager.Instance.m_ObjectPool.ReturnObject(this.gameObject.name, this.gameObject, Release);
    }

    public void Release()
    {
        this.transform.position = new Vector2(0, 0);
    }
}
