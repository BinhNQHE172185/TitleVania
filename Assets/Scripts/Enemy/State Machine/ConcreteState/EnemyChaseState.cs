using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    Transform player;
    bool canJump = true;
    float jumpCooldown = 3f;
    float jumpTimer = 0f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.moveSpeed = Mathf.Abs(enemy.moveSpeed);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 directionToPlayer = (player.position - enemy.transform.position).normalized;
        enemy.FacePlayer(directionToPlayer);
        if (canJump)
        {
            if (enemy.myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Vector2 jumpDirection = new Vector2(directionToPlayer.x * enemy.moveSpeed * 5, 15f).normalized;
                enemy.myRigidbody.velocity = jumpDirection * enemy.jumpForce;
                canJump = false;
            }
        }
        else
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpCooldown)
            {
                canJump = true;
                jumpTimer = 0f;
            }
            enemy.myRigidbody.velocity = new Vector2(directionToPlayer.x * enemy.moveSpeed, enemy.myRigidbody.velocity.y);
        }
    }

    public override void Update()
    {
        base.Update();
        if (!enemy.IsAggroed)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        enemy.DealDamage(collision, enemy.damage, enemy.knockbackForce);
    }
}
