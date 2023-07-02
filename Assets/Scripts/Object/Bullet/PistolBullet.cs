using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _destroyTime = 1f;

    private void OnEnable()
    {
        if (IsInvoking())
            CancelInvoke();

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
        GameManager.Instance.m_ObjectPoolManager.ReturnObject(this.gameObject);
    }

    public void Release()
    {
        this.transform.position = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hurtTarget = other.GetComponent<IDamageable>();
        if (hurtTarget != null && other.gameObject.name != "Player")
        {
            hurtTarget.Damage();
            Hide();
        }
    }
}
