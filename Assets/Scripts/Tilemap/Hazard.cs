using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damage = 150f; // Damage dealt by the hazard
    [SerializeField] private float knockbackForce = 20f; // Knockback force applied to the player
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched");
        DealDamage(collision, damage, knockbackForce);
    }

    public void DealDamage(Collision2D collision, float damage, float knockbackForce)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (!player.isImmune)
            {
                // Apply knockback to the player
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
                //playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                playerRigidbody.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
                // Apply damage to the player
                player.TakeDamage(damage);
            }
        }
    }
}
