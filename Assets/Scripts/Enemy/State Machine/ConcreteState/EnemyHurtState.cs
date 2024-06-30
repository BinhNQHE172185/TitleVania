using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private float hurtTimer;
    public EnemyHurtState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        hurtTimer = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        // Increment the timer
        hurtTimer += Time.deltaTime;

        // Check if the timer has reached the duration
        if (hurtTimer >= enemy.hurtDuration)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
}
