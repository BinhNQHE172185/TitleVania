using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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
        IgnoreTilemapCollisions(true);

    }

    public override void ExitState()
    {
        base.ExitState();
        player.myRigidbody.velocity = Vector2.zero;
        player.myRigidbody.gravityScale = player.gravityScaleAtStart;
        player.myAnimator.SetBool("isDashing", false);
        IgnoreEnemyCollisions(false);
        IgnoreTilemapCollisions(false);
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
        CapsuleCollider2D playerColliders = player.GetComponent<CapsuleCollider2D>();

        // Get all enemy colliders in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D[] enemyColliders = enemy.GetComponents<Collider2D>();
            foreach (var enemyCollider in enemyColliders)
            {
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(playerColliders, enemyCollider, ignore);

                }
            }

        }
    }
    void IgnoreTilemapCollisions(bool ignore)
    {
        // Get the player's collider
        CapsuleCollider2D playerCollider = player.GetComponent<CapsuleCollider2D>();

        // Layers to ignore collisions with
        string[] layersToIgnore = new string[] { "Hazards", "Bouncing" };

        foreach (string layerName in layersToIgnore)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                Debug.LogWarning($"Layer '{layerName}' not found. Make sure it exists in the project settings.");
                continue;
            }

            // Find all Tilemaps with the specified layer
            Tilemap[] tilemaps = GameObject.FindObjectsOfType<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap.gameObject.layer == layer)
                {
                    // Get the collider attached to the tilemap
                    Collider2D tilemapCollider = tilemap.GetComponent<Collider2D>();
                    if (tilemapCollider != null)
                    {
                        Physics2D.IgnoreCollision(playerCollider, tilemapCollider, ignore);
                    }
                }
            }
        }
    }

}
