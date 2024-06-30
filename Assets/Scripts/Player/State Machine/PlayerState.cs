using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) { }
    public virtual void OnFire(InputValue value) { }
    public virtual void OnMove(InputValue value) { }
    public virtual void OnJump(InputValue value) { }
    public virtual void OnDash(InputValue value) { }
}
