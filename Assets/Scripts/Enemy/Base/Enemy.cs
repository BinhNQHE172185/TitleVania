using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IEnemyMovable, ITriggerAggroCheckable, ITriggerEdgeCheckable, IDealDamagable
{
    [field: SerializeField] public float HP { get; set; } = 1000;
    public float RemainingHP { get; set; }

    [field: SerializeField] public float damage { get; set; } = 100f;
    [field: SerializeField] public float knockbackForce { get; set; } = 100f; // Force of the knockback
    [field: SerializeField] public float hurtDuration { get; set; } = 0.5f;
    public Animator myAnimator { get; set; }
    [field: SerializeField] public HealthBarAction healthBarAction { get; set; }
    [field: SerializeField] public float moveSpeed { get; set; }
    [field: SerializeField] public float jumpForce { get; set; }
    [field: SerializeField] public float detectionRange { get; set; }
    public Rigidbody2D myRigidbody { get; set; }
    public BoxCollider2D myFeetCollider { get; set; }
    public float jumpCooldown { get; set; }
    public float jumpTimer { get; set; }
    [field: SerializeField] public GameObject body { get; set; }
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        EnemyDead,
        PlayFootstepSound
    }
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyIdleState idleState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyHurtState hurtState { get; set; }
    public EnemyDeadState deadState { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsInRange { get; set; }
    public bool IsOutsideOfEdge { get; set; }

    public bool IsFacingWall { get; set; }
    public bool isGrounded { get; set; }

    void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        hurtState = new EnemyHurtState(this, stateMachine);
        deadState = new EnemyDeadState(this, stateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        RemainingHP = HP;
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.CurrentEnemyState.Update();
    }
    void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.FixedUpdate();
        isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Hazards"));
        if (!isGrounded)
        {
            UpdateAirborne();
        }
        else { myAnimator.speed = 1; }
    }
    void UpdateAirborne()
    {
        // Adjust the animation based on the enemy's movement speed
        float time = Map(myRigidbody.velocity.y, jumpForce, -jumpForce, 0, 1, true);
        myAnimator.Play("Goober Jump", 0, time);
        myAnimator.speed = 0;
    }
    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }
    public void Die()
    {
        stateMachine.ChangeState(deadState);
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
    /*
    public IEnumerator TakeKnockBack(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the duration
        if(stateMachine.CurrentEnemyState != deadState)
        {
            stateMachine.ChageState(idleState);
        }
    }
    */

    public void UpdateHP()
    {
        healthBarAction.UpdateHealthBar(RemainingHP, HP);
    }

    public void FlipEnemyFacing()
    {
        body.transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    public void FacePlayer(Vector2 directionToPlayer)
    {
        if (directionToPlayer.x > 0)
        {
            body.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (directionToPlayer.x < 0)
        {
            body.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void FaceForward()
    {
        body.transform.localScale = new Vector2((Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        stateMachine.CurrentEnemyState.OnTriggerExit2D(other);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.CurrentEnemyState.OnCollisionEnter2D(collision);
    }

    public void DealDamage(Collision2D collision, float damage, float knockbackForce)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Apply damage to the player
                player.TakeDamage(damage);

                // Apply knockback to the player
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            */
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // Apply knockback to the player
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                playerRigidbody.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
                // Apply damage to the player
                player.TakeDamage(damage);
            }
        }
    }

    public void SetAggroStatus(bool IsAggroed)
    {
        this.IsAggroed = IsAggroed;
    }

    public void SetInRangeStatus(bool IsInRange)
    {
        this.IsInRange = IsInRange;
    }

    public void SetOutsideOfEdgeStatus(bool IsOutsideOfEdge)
    {
        this.IsOutsideOfEdge = IsOutsideOfEdge;
    }
    public void SetFacingWallStatus(bool IsFacingWall)
    {
        this.IsFacingWall = IsFacingWall;
    }
}
