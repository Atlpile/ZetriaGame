using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    protected Animator anim;

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
