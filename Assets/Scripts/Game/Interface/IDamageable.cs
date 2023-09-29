using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// 受到攻击时做的事情
    /// </summary>
    /// <param name="attacker">攻击者的位置</param>
    void OnDamage(Vector2 attacker);
}
