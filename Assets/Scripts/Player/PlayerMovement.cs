using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject body;
    [SerializeField] GameObject climbPlatform;
    [SerializeField] Transform gun;
    [SerializeField] HealthBarAction healthBarAction;
    [SerializeField] ChargeBarAction chargeBarAction;
    [SerializeField] float knockbackDuration = 0.3f; // Duration of the knockback effect
    [SerializeField] float takeDMGImmuneTime = 1f;
    [SerializeField] float dashDMGImmuneTime = 2f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Rigidbody2D climbPlatformRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    InputHandler playerInputHandler;
    private Tilemap ladderTilemap;


    enum PlayerState { Idle, Run, Charge, Dash, Hurt }
    PlayerState currentState = PlayerState.Idle;

    bool isCharging = false;
    bool canShoot = false;
    bool isMoving = false;
    bool isDashing = false;
    [SerializeField] bool isImmune = false;
    bool isClimbing = false;
    float dashTimer = 0f;
    float dashCoolDown = 2f;
    int dashCount = 2;
    int dashCountMax = 4;
    float chargeStartTime = 0f;
    const float chargeTime = 0.5f;
    float HP = 1000;
    public float RemainingHP;




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
        climbPlatformRigidBody = climbPlatform.GetComponent<Rigidbody2D>();
        climbPlatform.SetActive(false);
        ladderTilemap = GameObject.FindGameObjectWithTag("Climbing").GetComponent<Tilemap>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        //CollideCheck();
        MoveLadder();
        RecoverDash();
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
            chargeBarAction.ClearChargeBar();
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
            canShoot = false;
            chargeStartTime = Time.time;
            myAnimator.SetBool("isCharging", true);
            myAnimator.SetBool("canShoot", false);
        }
        else
        {
            // Update charge duration
            float chargeDuration = Time.time - chargeStartTime;
            chargeBarAction.UpdateChargeBar(chargeDuration, chargeTime);

            if (chargeDuration >= chargeTime)
            {
                canShoot = true;
                myAnimator.SetBool("canShoot", true);
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
            }
            // Reset the charging state
            isCharging = false;
            chargeBarAction.ClearChargeBar();
            myAnimator.SetBool("isCharging", false);
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
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing", "Enemies"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    void OnDash(InputValue value)
    {
        Debug.Log("Shifted");
        if (!isAlive) { return; }
        if (value.isPressed && dashCount > 0)
        {
            Debug.Log("Dashing");
            isDashing = true;

            myAnimator.SetBool("isDashing", true);

            StartCoroutine(StartImmunity(dashDMGImmuneTime));

            float dashDirection = body.transform.localScale.x > 0 ? 1f : -1f;

            // Apply dash force in the direction
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(new Vector2(dashSpeed * dashDirection, myRigidbody.velocity.y), ForceMode2D.Impulse);
            dashCount--;

            Invoke("EndDash", 0.3f);
        }
    }
    void EndDash()
    {
        myAnimator.SetBool("isDashing", false);
        isDashing = false;
    }
    void RecoverDash()
    {
        if (dashCount < dashCountMax)
        {
            // Increment the dash timer
            dashTimer += Time.deltaTime;

            // If the dash timer exceeds the cooldown period
            if (dashTimer >= dashCoolDown)
            {
                // Increment the dash count
                dashCount++;

                // Reset the dash timer
                dashTimer = 0f;
            }
        }
    }

    void Run()
    {
        if (isDashing) { return; }
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            AttachToPlatform(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbing"))
        {
            DetachFromPlatform();
        }
    }

    void AttachToPlatform(Collider2D ladderCollider)
    {
        //make player attach to ladder
        isClimbing = true;
        myRigidbody.gravityScale = gravityScaleAtStart;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        climbPlatform.SetActive(true);
        // Get the cell position of the tile the player is colliding with
        Vector3Int cellPosition = ladderTilemap.WorldToCell(transform.position);
        // Get the world position of the center of that tile
        Vector3 tileWorldPosition = ladderTilemap.GetCellCenterWorld(cellPosition);

        // Adjust the platform's position based on the tile's position
        float playerHeight = myBodyCollider.bounds.extents.y;
        float platformYPosition = tileWorldPosition.y - playerHeight - 0.2f;
        float platformXPosition = tileWorldPosition.x;

        climbPlatform.transform.position = new Vector2(platformXPosition, platformYPosition);
    }
    void MoveLadder()
    {
        if (!isClimbing)
        {
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        // Get the cell position of the tile the player is colliding with
        Vector3Int cellPosition = ladderTilemap.WorldToCell(transform.position);
        // Get the world position of the center of that tile
        Vector3 tileWorldPosition = ladderTilemap.GetCellCenterWorld(cellPosition);

        // Adjust the platform's position based on the tile's position
        float platformXPosition = tileWorldPosition.x;
        float verticalMove = moveInput.y * climbSpeed;

        climbPlatformRigidBody.velocity = new Vector3(0, verticalMove, 0);
        bool playerHasVerticalSpeed = Mathf.Abs(climbPlatformRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        //isMoving = playerHasVerticalSpeed;
        //maintain platform horizonal position
        if (climbPlatform.transform.position.y <= transform.position.y - 1f)
        {
            // Prevent downward movement
            climbPlatform.transform.position = new Vector2(platformXPosition, transform.position.y - 1f);
        }
        else
        {
            climbPlatform.transform.position = new Vector2(platformXPosition, climbPlatform.transform.position.y);
        }
    }

    void DetachFromPlatform()
    {
        isClimbing = false;
        climbPlatform.SetActive(false);
    }
    /*
    void CollideCheck()
    {
        if (isImmune) { Debug.Log("Immuned"); return; }

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            TakeDamage(200, 1);
            Debug.Log("Hit " + RemainingHP);
        }
        else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            TakeDamage(200, 0.5f);
            Debug.Log("Hit " + RemainingHP);
        }
    }
    */
    public void TakeDamage(float damage)
    {
        if (isImmune) { Debug.Log("Immuned"); return; }
        RemainingHP -= damage;
        //ApplyKnockback(5f); // Apply knockback effect
        StartCoroutine(StartImmunity(takeDMGImmuneTime));
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

    private IEnumerator StartImmunity(float duration)
    {
        isImmune = true; // Enable immunity
        yield return new WaitForSeconds(duration); // Wait for the duration
        isImmune = false; // Disable immunity
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
