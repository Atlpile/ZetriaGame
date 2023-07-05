using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet循环过程：Create —— Move —— Explosion —— Release —— Hide —— Create

[RequireComponent(typeof(Rigidbody2D))]
public class BaseBullet : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D coll2d;
    [SerializeField] protected float moveSpeed = 20f;
    protected float currentMoveSpeed;
    [SerializeField] protected float explosionTime = 1f;
    [SerializeField] protected float disappearTime = 1f;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        coll2d = this.GetComponent<Collider2D>();

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
        InitBullet();
    }

    protected virtual void InitComponent()
    {

    }

    protected virtual void InitBullet()
    {

    }

    protected virtual void Create()
    {
        StartCoroutine(IE_BaseDisappear());
    }

    protected virtual void Hide()
    {
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(this.gameObject);
    }

    protected virtual void Release()
    {
        this.transform.position = Vector2.zero;
        StopCoroutine(IE_BaseDisappear());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.Damage();
            Hide();
        }
    }

    protected virtual void Explosion()
    {

    }

    protected void ChangeMove(bool canStop)
    {
        if (canStop == true)
            currentMoveSpeed = 0;
        else
            currentMoveSpeed = moveSpeed;

    }

    public void InitBulletPostion(Vector3 positon)
    {
        this.transform.position = positon;
    }

    private IEnumerator IE_BaseDisappear()
    {
        yield return new WaitForSeconds(disappearTime);
        Hide();
    }


}
