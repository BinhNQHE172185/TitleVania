using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void Update()
    {

        base.Update();
        if (enemy.IsAggroed)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (enemy.IsOutsideOfEdge || enemy.IsFacingWall)
        {
            enemy.moveSpeed = -enemy.moveSpeed;
            enemy.FlipEnemyFacing();
        }
        enemy.myRigidbody.velocity = new Vector2(enemy.moveSpeed, enemy.myRigidbody.velocity.y);
        enemy.FaceForward();

    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        enemy.DealDamage(collision, enemy.damage, enemy.knockbackForce);
    }

}
