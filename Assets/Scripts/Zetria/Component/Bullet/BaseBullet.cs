using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

//Bullet循环过程：Create —— Move —— Explosion —— Release —— Hide —— Create
namespace Zetria
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public abstract class BaseBullet : BaseComponent, IPoolObject
    {
        public override IGameStructure GameStructure => ZetriaGame.Instance;

        protected Animator anim;
        protected Rigidbody2D rb2d;
        protected Collider2D coll2d;
        protected IDamageable damageable;

        //TODO:使用配置表或SO配置数据
        [SerializeField] protected float moveSpeed = 20f;
        protected float currentMoveSpeed;
        [SerializeField] protected float explosionTime = 1f;
        [SerializeField] protected float disappearTime = 0.5f;
        private bool _isExplosion;


        public virtual void OnInit()
        {
            anim = this.GetComponent<Animator>();
            rb2d = this.GetComponent<Rigidbody2D>();
            coll2d = this.GetComponent<Collider2D>();

            coll2d.isTrigger = true;
        }

        public virtual void OnCreate()
        {
            _isExplosion = false;
            coll2d.enabled = true;
            SetMoveStatus(false);
            StartCoroutine(IE_Disappear());
        }

        public virtual void OnRelease()
        {
            this.transform.position = Vector2.zero;
            StopAllCoroutines();
        }

        public virtual void OnReturn()
        {
            Manager.GetManager<IObjectPoolManager>().ReturnObject(this.gameObject);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null && other.gameObject.name == "Player")
            {
                damageable.OnDamage(this.transform.position);
                StartCoroutine(IE_TriggerExplosion());
            }

            if (other.CompareTag("Ground"))
            {
                StartCoroutine(IE_TriggerExplosion());
            }
        }

        protected virtual void Explosion()
        {
            if (!_isExplosion)
            {
                _isExplosion = true;

                Manager.GetManager<IAudioManager>().AudioPlay(FrameCore.E_AudioType.Effect, "bullet_blast");
                anim.Play("Explosion");
                coll2d.enabled = false;
                SetMoveStatus(true);
            }

        }

        protected void SetMoveStatus(bool canStop)
        {
            if (canStop == true)
                currentMoveSpeed = 0;
            else
                currentMoveSpeed = moveSpeed;
        }

        protected IEnumerator IE_Disappear()
        {
            yield return new WaitForSeconds(explosionTime);
            Explosion();

            yield return new WaitForSeconds(disappearTime);
            OnReturn();
        }

        protected IEnumerator IE_TriggerExplosion()
        {
            Explosion();

            yield return new WaitForSeconds(disappearTime);
            OnReturn();
        }


    }
}

