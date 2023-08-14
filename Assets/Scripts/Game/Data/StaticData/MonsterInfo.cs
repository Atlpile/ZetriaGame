using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public E_MonsterType monsterType = E_MonsterType.Null;

    public int maxHealth = 3;
    public float groundSpeed = 3f;
    public float airSpeed = 2f;
    public float attackDuration = 2f;
    public int attackDamage = 1;
    public float attackDistance = 1f;

    public Vector3 checkSize = new Vector2(7, 2);
    public Vector3 checkOffset;
    public float checkRadius = 1f;
}
