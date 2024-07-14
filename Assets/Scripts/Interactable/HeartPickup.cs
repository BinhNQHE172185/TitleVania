using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    bool wasCollected = false;
    float healAmmount = 500;
    AudioManager manager;
    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            manager.PlaySFX(manager.heart);
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
