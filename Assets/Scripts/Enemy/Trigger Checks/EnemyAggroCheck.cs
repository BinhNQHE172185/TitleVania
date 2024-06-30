using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    public GameObject player { get; set; }
    private Enemy enemy { get; set; }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponentInParent<Enemy>();
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = enemy.detectionRange;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            enemy.SetAggroStatus(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            enemy.SetAggroStatus(false);
        }
    }
}
