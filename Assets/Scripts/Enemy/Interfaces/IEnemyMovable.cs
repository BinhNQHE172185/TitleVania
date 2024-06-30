using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovable
{
    float moveSpeed { get; set; }
    float jumpForce { get; set; }
    float detectionRange { get; set; }
    Rigidbody2D myRigidbody { get; set; }
    BoxCollider2D myFeetCollider { get; set; }
    GameObject body { get; set; }

    float jumpCooldown { get; set; }
    float jumpTimer { get; set; }
    void FlipEnemyFacing();
    void FacePlayer(Vector2 directionToPlayer);
    void FaceForward();
}
