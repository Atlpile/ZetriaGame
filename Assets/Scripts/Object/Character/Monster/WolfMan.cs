using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    写显示逻辑
    写专有组件逻辑
*/

[RequireComponent(typeof(CapsuleCollider2D))]
public class WolfMan : BaseMonster
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector3 size;

    public override void InitComponent()
    {
        check = this.transform.GetChild(0);
    }

    protected override void InitCharacter()
    {
        fsm.ChangeState(E_AIState.Idle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, size);
    }
}
