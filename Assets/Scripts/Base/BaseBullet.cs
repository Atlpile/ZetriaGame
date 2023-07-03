using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseBullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float moveSpeed = 20f;
    protected float destroyTime = 1f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        InitComponent();
    }

    private void OnEnable()
    {
        Create();
    }

    private void OnDisable()
    {
        Release();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }


    protected virtual void InitComponent()
    {

    }

    protected virtual void InitBullet()
    {

    }

    protected virtual void Create()
    {

    }

    protected virtual void Hide()
    {

    }

    protected virtual void Release()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void InitBulletPostion(Vector3 positon)
    {
        this.transform.position = positon;
    }
}
