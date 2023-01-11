using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pistol : BaseObject
{
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _destroyTime = 1f;

    private void OnEnable()
    {
        Invoke("Hide", _destroyTime);
    }

    protected override void OnUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
    }


    public void Hide()
    {
        GameManager.Instance.ObjectPool.ReturnObject(this.gameObject.name, this.gameObject, Release);
    }

    public void Release()
    {
        this.transform.position = new Vector2(0, 0);
    }
}
