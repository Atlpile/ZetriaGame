using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected Animator anim;
    // protected bool bulletCanDestroyed;
    // public bool BulletCanDestroyed => bulletCanDestroyed;

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }


    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }
}
