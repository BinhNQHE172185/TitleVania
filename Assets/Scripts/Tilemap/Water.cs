using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Player player;
    private Rigidbody2D playerRigidbody;
    private float originalGravityScale;
    private float originalDrag;

    [Header("Water Effect Settings")]
    [SerializeField] float waterGravityScale = 1f; // Gravity scale while in water
    [SerializeField] public float waterDrag = 5.0f; // Drag while in water

    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        originalGravityScale = playerRigidbody.gravityScale;
        originalDrag = playerRigidbody.drag;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            EnterWater();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            ExitWater();
        }
    }

    void EnterWater()
    {
        playerRigidbody.gravityScale = waterGravityScale; // reduce gravity to simulate buoyancy
        playerRigidbody.drag = waterDrag; // increase drag to simulate water resistance
    }

    void ExitWater()
    {
        playerRigidbody.gravityScale = originalGravityScale; // reset gravity scale
        playerRigidbody.drag = originalDrag; // reset drag
    }
}
