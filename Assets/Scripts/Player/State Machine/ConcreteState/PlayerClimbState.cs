using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerClimbState : PlayerState
{
    public PlayerClimbState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        player.myRigidbody.gravityScale = player.gravityScaleAtStart;
        player.myRigidbody.velocity = new Vector2(player.myRigidbody.velocity.x, 0f);
        player.climbPlatform.SetActive(true);
        // Get the cell position of the tile the player is colliding with
        Vector3Int cellPosition = player.ladderTilemap.WorldToCell(player.transform.position);
        // Get the world position of the center of that tile
        Vector3 tileWorldPosition = player.ladderTilemap.GetCellCenterWorld(cellPosition);

        // Adjust the platform's position based on the tile's position
        float playerHeight = player.myBodyCollider.bounds.extents.y;
        float platformYPosition = player.transform.position.y - playerHeight - player.climbPlatform.GetComponent<BoxCollider2D>().bounds.size.y;
        float platformXPosition = tileWorldPosition.x;
        player.climbPlatform.transform.position = new Vector2(platformXPosition, platformYPosition);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.myAnimator.SetBool("isClimbing", false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!player.isClimbing)
        {
            player.climbPlatform.SetActive(false);
            player.stateMachine.ChangeState(player.moveState);
        }
        MoveLadder();
        Jump();
        Run();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    public override void OnDash(InputValue value)
    {
        player.climbPlatform.SetActive(false);
        player.stateMachine.ChangeState(player.dashState);
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
    void MoveLadder()
    {
        // Get the cell position of the tile the player is colliding with
        Vector3Int cellPosition = player.ladderTilemap.WorldToCell(player.transform.position);
        // Get the world position of the center of that tile
        Vector3 tileWorldPosition = player.ladderTilemap.GetCellCenterWorld(cellPosition);

        // Adjust the platform's position based on the tile's position
        float platformXPosition = tileWorldPosition.x;
        float verticalMove = player.moveInput.y * player.climbSpeed;

        player.climbPlatformRigidBody.velocity = new Vector3(0, verticalMove, 0);
        bool playerHasVerticalSpeed = Mathf.Abs(player.climbPlatformRigidBody.velocity.y) > Mathf.Epsilon;
        player.myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        //isMoving = playerHasVerticalSpeed;
        //maintain platform horizonal position
        if (player.climbPlatform.transform.position.y <= player.transform.position.y - 1f)
        {
            // Prevent downward movement
            player.climbPlatform.transform.position = new Vector2(platformXPosition, player.transform.position.y - 1f);
        }
        else
        {
            player.climbPlatform.transform.position = new Vector2(platformXPosition, player.climbPlatform.transform.position.y);
        }
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
        if (player.isGrounded)
        {
            player.myAnimator.speed = 1;
            if (player.playerInputHandler.jumpTriggered)
            {
                player.myRigidbody.velocity += new Vector2(0f, player.jumpSpeed);
            }
            else
            {
                player.stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            player.UpdateAirborne();
        }
    }
}
