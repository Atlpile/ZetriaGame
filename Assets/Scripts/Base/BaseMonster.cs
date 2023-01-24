using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : BaseCharacter
{
    protected AILogic aiLogic;

    protected override void OnAwake()
    {
        base.OnAwake();

        Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (aiLogic != null)
            aiLogic.UpdateState();
    }

    public abstract void Init();
}
