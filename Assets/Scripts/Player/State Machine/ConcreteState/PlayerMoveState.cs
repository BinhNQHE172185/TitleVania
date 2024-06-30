using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
        player.myAnimator.SetBool("isRunning", false);
    }

    public override void FixedUpdate()
    {
        if (player.isClimbing)
        {
            player.stateMachine.ChangeState(player.climbState);
        }
        Run();
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
    }

    public override void OnJump(InputValue value)
    {
        player.stateMachine.ChangeState(player.jumpState);
    }

    public override void OnMove(InputValue value)
    {
        player.moveInput = value.Get<Vector2>();
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
        if (player.playerInputHandler.fireTriggered)
        {
            player.stateMachine.ChangeState(player.fireState);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(player.moveInput.x * player.runSpeed, player.myRigidbody.velocity.y);

        player.myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(player.myRigidbody.velocity.x) > Mathf.Epsilon;
        player.isMoving = playerHasHorizontalSpeed;
        player.myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        if (!playerHasHorizontalSpeed)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
