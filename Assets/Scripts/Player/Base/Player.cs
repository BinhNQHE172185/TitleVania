using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [field: SerializeField] public float runSpeed { get; set; } = 10f;
    [field: SerializeField] public float jumpSpeed { get; set; } = 5f;
    [field: SerializeField] public float dashSpeed { get; set; } = 5f;
    [field: SerializeField] public float climbSpeed { get; set; } = 5f;
    [field: SerializeField] public float hurtDuration { get; set; } = 0.3f;
    [field: SerializeField] public GameObject bullet { get; set; }
    [field: SerializeField] public GameObject body { get; set; }
    [field: SerializeField] public GameObject climbPlatform { get; set; }
    [field: SerializeField] public Transform gun { get; set; }
    [field: SerializeField] public HealthBarAction healthBarAction { get; set; }
    [field: SerializeField] public ChargeBarAction chargeBarAction { get; set; }
    [field: SerializeField] public float knockbackDuration { get; set; } = 0.3f; // Duration of the knockback effect
    [field: SerializeField] public float takeDMGImmuneTime { get; set; } = 1f;
    [field: SerializeField] public float dashDMGImmuneTime { get; set; } = 2f;

    public Vector2 moveInput { get; set; }
    public Rigidbody2D myRigidbody { get; set; }
    public Rigidbody2D climbPlatformRigidBody { get; set; }
    public Animator myAnimator { get; set; }
    public CapsuleCollider2D myBodyCollider { get; set; }
    public BoxCollider2D myFeetCollider { get; set; }
    public float gravityScaleAtStart { get; set; }
    public InputHandler playerInputHandler { get; set; }
    public Tilemap ladderTilemap { get; set; }

    public GameSession gameSession { get; set; }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        stateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        EnemyDead,
        PlayFootstepSound
    }
    public PlayerStateMachine stateMachine { get; set; }
    public PlayerIdleState idleState { get; set; }
    public PlayerMoveState moveState { get; set; }
    public PlayerJumpState jumpState { get; set; }
    public PlayerClimbState climbState { get; set; }
    public PlayerDashState dashState { get; set; }
    public PlayerFireState fireState { get; set; }
    public PlayerHurtState hurtState { get; set; }
    public PlayerDeadState deadState { get; set; }

    public bool isAlive { get; set; } = true;
    public bool isCharging { get; set; } = false;
    public bool canShoot { get; set; } = false;
    public bool isMoving { get; set; } = false;
    [field: SerializeField] public bool isGrounded { get; set; } = true;
    public bool isDashing { get; set; } = false;
    public bool isImmune { get; set; } = false;
    public bool isClimbing { get; set; } = false;
    public float dashDuration { get; set; } = 0.4f;
    public float dashTimer { get; set; } = 0f;
    public float dashCoolDown { get; set; } = 2f;
    public int dashCount { get; set; } = 2;
    public int dashCountMax { get; set; } = 4;
    public float chargeStartTime { get; set; } = 0f;
    public float chargeTime { get; set; } = 0.5f;
    public float HP { get; set; } = 1000;
    public float RemainingHP { get; set; }


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine);
        moveState = new PlayerMoveState(this, stateMachine);
        jumpState = new PlayerJumpState(this, stateMachine);
        climbState = new PlayerClimbState(this, stateMachine);
        dashState = new PlayerDashState(this, stateMachine);
        fireState = new PlayerFireState(this, stateMachine);
        hurtState = new PlayerHurtState(this, stateMachine);
        deadState = new PlayerDeadState(this, stateMachine);
    }

    // Start is called before the first frame update
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
        gameSession = FindObjectOfType<GameSession>();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(stateMachine.CurrentPlayerState);
        stateMachine.CurrentPlayerState.Update();
        //FlipSprite();
        RecoverDash();
    }
    private void FixedUpdate()
    {
        /*
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing", "Enemies")))
        {

            player.stateMachine.ChangeState(moveState);
        }
        else
        {
            player.isGrounded = false;
        }
        */
        stateMachine.CurrentPlayerState.FixedUpdate();
    }
    public void TakeDamage(float damage)
    {
        stateMachine.ChangeState(hurtState);
        RemainingHP -= damage;
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
    public void UpdateHP()
    {
        healthBarAction.UpdateHealthBar(RemainingHP, HP);
    }
    public void Die()
    {
        stateMachine.ChangeState(deadState);
    }
    public void OnFire(InputValue value)
    {
        stateMachine.CurrentPlayerState.OnFire(value);
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        FlipSprite();
        stateMachine.CurrentPlayerState.OnMove(value);
    }
    public void OnJump(InputValue value)
    {
        stateMachine.CurrentPlayerState.OnJump(value);
    }
    public void OnDash(InputValue value)
    {
        stateMachine.CurrentPlayerState.OnDash(value);
    }
    public IEnumerator StartImmunity(float duration)
    {
        isImmune = true; // Enable immunity
        yield return new WaitForSeconds(duration); // Wait for the duration
        isImmune = false; // Disable immunity
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
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            body.transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }
}
