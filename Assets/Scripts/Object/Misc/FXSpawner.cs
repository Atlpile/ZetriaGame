using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : MonoBehaviour, IObject
{
    [SerializeField] private float _destroyTime = 0.5f;
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Create();
    }

    private void OnDisable()
    {
        Release();
    }

    public void Create()
    {
        animator.Play("Create");
        Hide();
    }

    public void Hide()
    {
        StartCoroutine(IE_Hide());
    }

    public void Release()
    {
        StopCoroutine(IE_Hide());
    }

    private IEnumerator IE_Hide()
    {
        yield return new WaitForSeconds(_destroyTime);
        GameManager.Instance.ObjectPoolManager.ReturnObject(this.gameObject);
    }
}
