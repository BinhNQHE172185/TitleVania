using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.myAnimator.SetBool("isDead", true);
        CapsuleCollider2D oldCollider = enemy.GetComponent<CapsuleCollider2D>();
        // Add a new BoxCollider2D
        BoxCollider2D newCollider = enemy.gameObject.AddComponent<BoxCollider2D>();
        newCollider.size = new Vector3(oldCollider.bounds.size.x * 2f, oldCollider.bounds.size.y, oldCollider.bounds.size.z); // Adjust size to match the enemy
        newCollider.offset = oldCollider.offset; // Adjust offset if needed

        // Set the mass of the Rigidbody2D
        enemy.myRigidbody.mass = 15f;
        oldCollider.enabled = false;

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
