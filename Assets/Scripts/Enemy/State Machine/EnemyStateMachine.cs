using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState {  get; set; }
    public void Initialize(EnemyState startEnemyState)
    {
        CurrentEnemyState = startEnemyState;
        CurrentEnemyState.EnterState();
    }
    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
    }
}
