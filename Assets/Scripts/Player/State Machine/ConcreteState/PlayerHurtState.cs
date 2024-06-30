using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHurtState : PlayerState
{
    private float hurtTimer;
    public PlayerHurtState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        hurtTimer = 0f;
        player.StartCoroutine(player.StartImmunity(player.takeDMGImmuneTime));
    }

    public override void ExitState()
    {
        base.ExitState();
        player.myRigidbody.velocity = Vector2.zero;

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
        base.OnDash(value);
    }

    public override void OnFire(InputValue value)
    {
        base.OnFire(value);
    }

    public override void OnJump(InputValue value)
    {
        base.OnJump(value);
    }

    public override void OnMove(InputValue value)
    {
        base.OnMove(value);
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
        // Increment the timer
        hurtTimer += Time.deltaTime;

        // Check if the timer has reached the duration
        if (hurtTimer >= player.hurtDuration)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
