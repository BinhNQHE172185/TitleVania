using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float bulletDmg = 500f;
    [SerializeField] ParticleSystem explodeParticle;
    [SerializeField] ParticleSystem trailParticle;

    Rigidbody2D myRigidbody;
    SpriteRenderer mySpriteRenderer;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        xSpeed = transform.localScale.x * bulletSpeed;
        explodeParticle.Stop();
        trailParticle.Play();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            // Apply knockback force to the enemy
            Rigidbody2D enemyRigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                EnemyMovement enemy = enemyRigidbody.GetComponent<EnemyMovement>();
                Debug.Log("hit slime");
                enemy.TakeDamage(bulletDmg);
            }
        }
        HandleCollision();
    }
    void HandleCollision()
    {
        // Hide the sprite renderer
        mySpriteRenderer.enabled = false;

        // Stop the trail particle system
        if (trailParticle != null)
        {
            trailParticle.Stop();
        }

        // Play the explode particle system
        if (explodeParticle != null)
        {
            explodeParticle.Play();
        }

        //destroy the bullet
        Invoke("DestroyAfterExplosion", 0.4f);
    }

    void DestroyAfterExplosion()
    {
        Destroy(gameObject);
    }

}
