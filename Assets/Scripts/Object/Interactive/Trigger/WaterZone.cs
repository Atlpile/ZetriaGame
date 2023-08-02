using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.Damage(this.transform.position);

    }

    //可优化：Camera接近时，所有水可以播放动画，否则不播放
}
