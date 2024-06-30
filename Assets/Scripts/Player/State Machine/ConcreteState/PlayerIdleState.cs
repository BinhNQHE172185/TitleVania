using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.myRigidbody.velocity = new Vector2(0,player.myRigidbody.velocity.y);//avoid sliding
        if (player.isClimbing)
        {
            player.stateMachine.ChangeState(player.climbState);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void OnDash(InputValue value)
    {
        if (player.dashCount > 0)
        {
            player.stateMachine.ChangeState(player.dashState);
            player.stateMachine.CurrentPlayerState.OnDash(value);
        }
    }

    public override void OnFire(InputValue value)
    {
        player.stateMachine.ChangeState(player.fireState);
        player.stateMachine.CurrentPlayerState.OnFire(value);
    }

    public override void OnJump(InputValue value)
    {
        player.stateMachine.ChangeState(player.jumpState);
    }

    public override void OnMove(InputValue value)
    {
        player.stateMachine.ChangeState(player.moveState);
        player.stateMachine.CurrentPlayerState.OnMove(value);
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
    }

}
