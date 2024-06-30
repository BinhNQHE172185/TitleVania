using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireState : PlayerState
{
    bool canShoot;
    float currentChargeTime;

    public PlayerFireState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        // Start charging
        canShoot = false;
        currentChargeTime = 0f;
        player.myAnimator.SetBool("isCharging", true);
        player.myAnimator.SetBool("canShoot", false);
        player.chargeBarAction.ClearChargeBar();
    }

    public override void ExitState()
    {
        base.ExitState();
        // Reset the charging state
        canShoot = false;
        player.myAnimator.SetBool("isCharging", false);
        player.myAnimator.SetBool("canShoot", false);
        player.chargeBarAction.ClearChargeBar();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.myRigidbody.velocity = Vector2.zero;

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
    }

    public override void OnJump(InputValue value)
    {
        player.stateMachine.ChangeState(player.jumpState);
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
        if (player.playerInputHandler.fireTriggered)
        {
            HandleCharging();
        }
        else
        {
            HandleShooting();
        }
    }

    private void HandleCharging()
    {
        // Update charge duration
        currentChargeTime += Time.deltaTime;
        player.chargeBarAction.UpdateChargeBar(currentChargeTime, player.chargeTime);

        if (currentChargeTime >= player.chargeTime)
        {
            canShoot = true;
            player.myAnimator.SetBool("canShoot", true);
        }
    }

    private void HandleShooting()
    {
        if (canShoot)
        {
            // Shoot the bullet
            GameObject bulletInstance = Object.Instantiate(player.bullet, player.gun.position, Quaternion.identity);
            // Maintain the original scale of the bullet
            Vector3 originalScale = bulletInstance.transform.localScale;
            // Flip the bullet to match the shooter's direction
            bulletInstance.transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * Mathf.Sign(player.body.transform.localScale.x), originalScale.y, originalScale.z);
        }
        player.stateMachine.ChangeState(player.moveState);
    }
}
