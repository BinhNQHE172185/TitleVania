using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ladder : MonoBehaviour
{
    GameObject player;
    Player playerScript;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            AttachToPlatform(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            DetachFromPlatform();
        }
    }
    void AttachToPlatform(Collider2D ladderCollider)
    {
        playerScript.isClimbing = true;
    }
    void DetachFromPlatform()
    {
        playerScript.isClimbing = false;
    }
}
