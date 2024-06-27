using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyState { Idle, Chasing, Dead, Hurt }
    EnemyState currentState = EnemyState.Idle;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float damage = 100f;
    [SerializeField] float knockbackForce = 100f; // Force of the knockback
    [SerializeField] LayerMask playerLayer;
    [SerializeField] HealthBarAction healthBarAction;
    [SerializeField] GameObject body;

    Rigidbody2D myRigidbody;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;
    Transform player;

    [SerializeField] bool canJump = true;
    float jumpCooldown = 3f;
    float jumpTimer = 0f;
    float HP = 1000;
    public float RemainingHP;

    void Start()
    {
        myFeetCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        RemainingHP = HP;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBehavior();
                break;
            case EnemyState.Chasing:
                ChasingBehavior();
                break;
            case EnemyState.Dead:
                DeadBehavior();
                break;
            case EnemyState.Hurt:
                DeadBehavior();
                break;
        }

        CheckState();
    }

    void IdleBehavior()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
    }

    void ChasingBehavior()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        FacePlayer(directionToPlayer);
        if (canJump)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Vector2 jumpDirection = new Vector2(directionToPlayer.x * moveSpeed * 5, 15f).normalized;
                myRigidbody.velocity = jumpDirection * jumpForce;
                canJump = false;
            }
        }
        else
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpCooldown)
            {
                canJump = true;
                jumpTimer = 0f;
            }
            myRigidbody.velocity = new Vector2(directionToPlayer.x * moveSpeed, myRigidbody.velocity.y);
        }
    }

    void DeadBehavior()
    {
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //change direction when walk to cliff
        if (currentState == EnemyState.Idle)
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Dead)
            return;
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // Apply damage to the player
                player.TakeDamage(damage);

                // Apply knockback to the player
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    void FlipEnemyFacing()
    {
        body.transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
    void FacePlayer(Vector2 directionToPlayer)
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
    void FaceForward()
    {
        body.transform.localScale = new Vector2((Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    void CheckState()
    {
        if (currentState == EnemyState.Dead || currentState == EnemyState.Hurt)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            currentState = EnemyState.Chasing;
            moveSpeed = Mathf.Abs(moveSpeed);
        }
        else
        {
            FaceForward();
            currentState = EnemyState.Idle;
        }
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeKnockBack(0.5f));
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
    private IEnumerator TakeKnockBack(float duration)
    {
        currentState = EnemyState.Hurt;
        yield return new WaitForSeconds(duration); // Wait for the duration
        if (currentState != EnemyState.Dead)
        {
            currentState = EnemyState.Idle;
        }
    }
    public void UpdateHP()
    {
        healthBarAction.UpdateHealthBar(RemainingHP, HP);
    }

    void Die()
    {
        myAnimator.SetBool("isDead", true);
        currentState = EnemyState.Dead;
    }
}
