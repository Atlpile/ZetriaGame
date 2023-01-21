using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BaseEntity
{
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _destroyTime = 1f;

    private void OnEnable()
    {
        Create();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
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
