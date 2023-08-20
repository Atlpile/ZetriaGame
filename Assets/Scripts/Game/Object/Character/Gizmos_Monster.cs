using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmos_Monster : MonoBehaviour
{
    public bool canDrawGizmos;
    private BaseMonster monster;

    private void Awake()
    {
        monster = this.GetComponent<BaseMonster>();
    }

    private void OnDrawGizmos()
    {
        if (canDrawGizmos && monster != null)
        {
            if (monster.IsFindPlayer)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            // switch (monster.gizmosType)
            // {
            //     case E_GizmosType.Rect:
            //         Gizmos.DrawWireCube(this.transform.position + monster.MonsterInfo.checkOffset, monster.MonsterInfo.checkSize);
            //         break;
            //     case E_GizmosType.Circle:
            //         Gizmos.DrawWireSphere(this.transform.position + monster.MonsterInfo.checkOffset, monster.MonsterInfo.checkRadius);
            //         break;
            //     case E_GizmosType.Null:
            //         break;
            // }
        }
    }
}
