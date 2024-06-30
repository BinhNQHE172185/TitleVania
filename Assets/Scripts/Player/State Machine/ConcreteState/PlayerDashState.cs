using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerState
{
    float dashTimer;
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        dashTimer = 0;
        player.myAnimator.SetBool("isDashing", true);

        player.StartCoroutine(player.StartImmunity(player.dashDMGImmuneTime));

        float dashDirection = player.body.transform.localScale.x > 0 ? 1f : -1f;

        // Apply dash force in the direction
        player.myRigidbody.gravityScale = 0;
        player.myRigidbody.velocity = Vector2.zero;
        player.myRigidbody.AddForce(new Vector2(player.dashSpeed * dashDirection, player.myRigidbody.velocity.y), ForceMode2D.Impulse);
        player.dashCount--;
        IgnoreEnemyCollisions(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.myRigidbody.velocity = Vector2.zero;
        player.myRigidbody.gravityScale = player.gravityScaleAtStart;
        player.myAnimator.SetBool("isDashing", false);
        IgnoreEnemyCollisions(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void OnDash(InputValue value)
    {
    }

    public override void OnFire(InputValue value)
    {
    }

    public override void OnJump(InputValue value)
    {
        base.OnJump(value);
    }

    public override void OnMove(InputValue value)
    {
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void Update()
    {
        base.Update();
        dashTimer += Time.deltaTime;

        if (dashTimer >= player.dashDuration)
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }
    void IgnoreEnemyCollisions(bool ignore)
    {
        // Get all colliders attached to the player
        Collider2D[] playerColliders = player.GetComponents<Collider2D>();

        // Get all enemy colliders in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                foreach (Collider2D playerCollider in playerColliders)
                {
                    Physics2D.IgnoreCollision(playerCollider, enemyCollider, ignore);
                }
            }
        }
    }

}
