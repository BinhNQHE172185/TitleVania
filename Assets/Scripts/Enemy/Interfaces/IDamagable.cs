using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    float HP { get; set; }
    float  RemainingHP { get; set; }
    float hurtDuration { get; set; }
    Animator myAnimator { get; set; }
    HealthBarAction healthBarAction { get; set; }
    void TakeDamage(float damage);
    void UpdateHP();
    void Die();
}
