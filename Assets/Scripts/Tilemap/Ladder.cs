using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ladder : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            AttachToPlatform(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            DetachFromPlatform();
        }
    }
    void AttachToPlatform(Collider2D ladderCollider)
    {
        player.isClimbing = true;
    }
    void DetachFromPlatform()
    {
        player.isClimbing = false;
    }
}
