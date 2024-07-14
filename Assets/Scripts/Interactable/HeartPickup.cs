using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    bool wasCollected = false;
    float healAmmount = 500;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            Player player = FindObjectOfType<Player>();
            if (player != null )
            {
                player.Heal(healAmmount);
            }
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
