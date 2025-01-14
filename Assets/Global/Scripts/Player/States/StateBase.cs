using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class StateBase
{
    protected Player Player { get; private set; }

    protected StateBase(Player player)
    {
        Player = player;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }

    // Input handling
    public virtual void Move(InputAction.CallbackContext ctx)
    {
        Player.movementInput = ctx.ReadValue<Vector2>();
        if (ctx.performed && Player.currentState != Player.states.Walking && Player.isGrounded)
        {
            Player.SetState(Player.states.Walking);
        }
        else if (ctx.canceled && Player.currentState == Player.states.Walking)
        {
            Player.SetState(Player.states.Idle);
        }
        else if (ctx.canceled && Player.currentState == Player.states.Walking)
        {
            Player.SetState(Player.states.Idle);
        }
    }

    public virtual void Sprint(InputAction.CallbackContext ctx) { }

    public virtual void Dodge(InputAction.CallbackContext ctx)
    {
        if (Player.playerStatistic.CanDodge.GetValueInt() <= 0) return;
        
        if (ctx.started)
        {
            Player.isHoldingDodge = true;
            if (Player.canDodgeRoll)
            {
                Player.SetState(Player.states.DodgeRoll);
            }
        }
        if (ctx.canceled)
        {
            Player.isHoldingDodge = false;
        }
    }

    public virtual void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Player.isHoldingJump = true;
            Player.SetState(Player.states.Jumping);
        }
        if (ctx.canceled)
        {
            Player.isHoldingJump = false;
        }

    }

    public virtual void Glide(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            Player.SetState(Player.states.Falling);
        }
    }

    public virtual void Crouch(InputAction.CallbackContext ctx) { }

    public virtual void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Player.SetState(Player.states.Attacking);
        }
    }

    public virtual void Hurt(Vector3 direction)
    {
        Player.hitDirection = direction;
        Player.SetState(Player.states.HurtState);
    }

    // general movement logic
    protected float ApplyGravity(float yValue)
    {
        if (yValue < Player.jumpVelocityFalloff || yValue > 0 && !Player.isHoldingJump && !Player.isHoldingDodge)
        {
            yValue += Player.fallMultiplier * Physics.gravity.y * Time.deltaTime;
            Player.debug_lineColor = Color.red;
        }
        else
        {
            Player.debug_lineColor = Color.yellow;
        }
        return yValue;
    }
}
