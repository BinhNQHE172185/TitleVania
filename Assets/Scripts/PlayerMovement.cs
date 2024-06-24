using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] HealthBarAction healthBarAction;
    [SerializeField] float knockbackDuration = 0.3f; // Duration of the knockback effect
    [SerializeField] float immunityDuration = 0.8f; // Duration of the immunity after being hit

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    InputHandler playerInputHandler;
    [SerializeField] GameObject body;

    bool isCharging = false;
    bool canShoot = false;
    bool isMoving = false;
    float chargeStartTime = 0f;
    const float chargeTime = 0.5f;
    float HP = 1000;
    public float RemainingHP;

    bool isImmune = false; // To track the immunity state


    bool isAlive = true;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        playerInputHandler = InputHandler.Instance;
        RemainingHP = HP;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        CollideCheck();
        OnFire();
    }

    public void OnFire()
    {
        if (!isAlive || isMoving)
        {
            // Reset the charging state
            isCharging = false;
            canShoot = false;
            myAnimator.SetBool("isCharging", false);
            myAnimator.SetBool("canShoot", false);
            Debug.Log("Reset charging state");
            return;
        }

        // Handle charging
        if (playerInputHandler.fireTriggered)
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
        if (!isCharging)
        {
            // Start charging
            isCharging = true;
            chargeStartTime = Time.time;
            myAnimator.SetBool("isCharging", true);
            Debug.Log("Charging started");
        }
        else
        {
            // Update charge duration
            float chargeDuration = Time.time - chargeStartTime;
            Debug.Log("Charging " + chargeDuration);

            if (chargeDuration >= chargeTime)
            {
                canShoot = true;
                myAnimator.SetBool("canShoot", true);
                Debug.Log("Charging complete, canShoot = true");
            }
        }
    }

    private void HandleShooting()
    {
        if (isCharging)
        {
            if (canShoot)
            {
                // Shoot the bullet
                GameObject bulletInstance = Instantiate(bullet, gun.position, Quaternion.identity);
                // Maintain the original scale of the bullet
                Vector3 originalScale = bulletInstance.transform.localScale;
                // Flip the bullet to match the shooter's direction
                bulletInstance.transform.localScale = new Vector3(Mathf.Abs(originalScale.x) * Mathf.Sign(body.transform.localScale.x), originalScale.y, originalScale.z);
                Debug.Log("Shot!");
            }

            // Reset the charging state
            isCharging = false;
            canShoot = false;
            myAnimator.SetBool("isCharging", false);
            myAnimator.SetBool("canShoot", false);
            Debug.Log("Reset charging state");
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            // do stuff
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        isMoving = playerHasHorizontalSpeed;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            body.transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        isMoving = playerHasVerticalSpeed;
    }

    void CollideCheck()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            TakeDamage(200,1);
            Debug.Log("Hit " + RemainingHP);
        }
        else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            TakeDamage(200, 0.5f);
            Debug.Log("Hit " + RemainingHP);
        }
    }
    public void TakeDamage(float damage, float forceScale)
    {
        RemainingHP -= damage;
        ApplyKnockback(forceScale); // Apply knockback effect
        StartCoroutine(StartImmunity()); // Start immunity timer
        if (RemainingHP <= 0)
        {
            UpdateHP();
            Die();
        }
        else
        {
            UpdateHP();
        }
    }
    private void ApplyKnockback(float scale)
    {
        // Apply a knockback force
        myRigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * deathKick.x, deathKick.y * scale);
    }

    private IEnumerator StartImmunity()
    {
        isImmune = true; // Set immunity to true
        myBodyCollider.enabled = false; // Disable the collider to make the player immune to further collisions
        yield return new WaitForSeconds(immunityDuration); // Wait for the immunity duration
        myBodyCollider.enabled = true; // Re-enable the collider
        isImmune = false; // Reset immunity state
    }
    public void UpdateHP()
    {
        healthBarAction.UpdateHealthBar(RemainingHP, HP);
    }
    void Die()
    {
        isAlive = false;
        myAnimator.SetTrigger("Dying");
        myRigidbody.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
