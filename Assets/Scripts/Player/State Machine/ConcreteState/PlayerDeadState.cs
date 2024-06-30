using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeadState : PlayerState
{
    float ReviveDelay = 1f;
    float delayTimer = 0f;
    public PlayerDeadState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        delayTimer = 0f;
        player.myAnimator.SetTrigger("Dying");
    }

    public override void ExitState()
    {
        base.ExitState();
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
        delayTimer += Time.deltaTime;
        if (delayTimer > ReviveDelay)
        {
            player.gameSession.ProcessPlayerDeath();
        }
    }

}
