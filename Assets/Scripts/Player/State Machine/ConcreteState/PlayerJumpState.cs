using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        player.myAnimator.SetBool("isRunning", false);
    }

    public override void FixedUpdate()
    {
        Run();
        Jump();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
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
    }

    public override void OnMove(InputValue value)
    {
        player.moveInput = value.Get<Vector2>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void Update()
    {
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(player.moveInput.x * player.runSpeed, player.myRigidbody.velocity.y);

        player.myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(player.myRigidbody.velocity.x) > Mathf.Epsilon;
        player.isMoving = playerHasHorizontalSpeed;
        player.myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    void Jump()
    {
        if (player.myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climb Platform", "Enemies")))
        {
            player.isGrounded = true;
        }
        else
        {
            player.isGrounded = false;
        }
        if (player.isGrounded == true)
        {
            if (player.playerInputHandler.jumpTriggered)
            {
                player.myRigidbody.velocity += new Vector2(0f, player.jumpSpeed);
            }
            else
            {
                player.stateMachine.ChangeState(player.moveState);
            }
        }
    }
}
