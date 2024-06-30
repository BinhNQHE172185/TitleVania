using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealDamagable
{
    float damage { get; set; }
    float knockbackForce { get; set; }
    void DealDamage(Collision2D collision, float damage, float knockbackForce);
}
