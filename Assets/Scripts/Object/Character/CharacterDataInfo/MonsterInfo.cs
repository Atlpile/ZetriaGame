using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterInfo
{
    public E_MonsterType monsterType;
    public int maxHealth = 5;
    public int currentHealth;
    public float groundSpeed = 3f;
    public float airSpeed;
    public float attackDuration = 2f;
    public float attackDamage;
    public float attackDistance = 1f;
    public float alertDistance;

    public float patrolTime = 3f;

    public Vector3 checkSize = new Vector3(7, 2, 0);
    public Vector3 checkOffset;
    public float checkRadius = 1f;
}
