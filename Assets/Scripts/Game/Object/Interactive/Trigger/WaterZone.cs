using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            damageable.OnDamage(this.transform.position);

    }

    //可优化：Camera接近时，所有水可以播放动画，否则不播放
}
