using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCheck : MonoBehaviour
{
    private Enemy enemy { get; set; }
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            enemy.SetFacingWallStatus(true);
        }
        Debug.Log(collision.ToString() + " " + true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            enemy.SetFacingWallStatus(false);
        }
    }

}
