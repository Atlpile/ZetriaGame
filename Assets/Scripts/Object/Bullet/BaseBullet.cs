using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet循环过程：Create —— Move —— Explosion —— Release —— Hide —— Create

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class BaseBullet : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D coll2d;
    [SerializeField] protected float moveSpeed = 20f;
    protected float currentMoveSpeed;
    [SerializeField] protected float explosionTime = 1f;
    [SerializeField] protected float disappearTime = 0.5f;
    private bool _isExplosion;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        coll2d = this.GetComponent<Collider2D>();

        InitExtraComponent();
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

    protected virtual void InitExtraComponent()
    {

    }

    protected virtual void InitBullet()
    {
        coll2d.isTrigger = true;
    }

    protected virtual void Create()
    {
        _isExplosion = false;
        coll2d.enabled = true;
        SetMoveStatus(false);
        StartCoroutine(IE_Disappear());
    }

    protected virtual void Hide()
    {
        GameManager.Instance.ObjectPoolManager.ReturnObject(this.gameObject);
    }

    protected virtual void Release()
    {
        this.transform.position = Vector2.zero;
        StopAllCoroutines();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && (other.gameObject.name == "Player" || other.gameObject.name == "Ground"))
        {
            damageable.Damage(this.transform.position);
            StartCoroutine(IE_TriggerExplosion());
        }
    }

    protected virtual void Explosion()
    {
        if (_isExplosion == false)
        {
            _isExplosion = true;
            //播放爆炸动画，消除Bullet的交互效果
            GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "bullet_blast");
            // Debug.Log("爆炸");
            anim.Play("Explosion");
            coll2d.enabled = false;
            SetMoveStatus(true);

            // rb.velocity = Vector2.zero;
        }

    }

    protected void SetMoveStatus(bool canStop)
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

    protected IEnumerator IE_Disappear()
    {
        yield return new WaitForSeconds(explosionTime);
        Explosion();

        yield return new WaitForSeconds(disappearTime);
        Hide();
    }

    protected IEnumerator IE_TriggerExplosion()
    {
        Explosion();

        yield return new WaitForSeconds(disappearTime);
        Hide();
    }

}
