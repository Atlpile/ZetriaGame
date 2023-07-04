using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseBullet : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    [SerializeField] protected float moveSpeed = 20f;
    [SerializeField] protected float destroyTime = 1f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();

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
        StartCoroutine(IE_BaseCreate());
    }

    protected virtual void Hide()
    {
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(this.gameObject);
    }

    protected virtual void Release()
    {
        this.transform.position = Vector2.zero;
        StopCoroutine(IE_BaseCreate());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.Damage();
            Debug.Log("player受伤");
        }
    }

    public void InitBulletPostion(Vector3 positon)
    {
        this.transform.position = positon;
    }

    private IEnumerator IE_BaseCreate()
    {
        yield return new WaitForSeconds(destroyTime);
        Hide();
    }
}
