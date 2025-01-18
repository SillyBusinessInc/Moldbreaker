using UnityEngine;
using UnityEngine.InputSystem;

public class HurtState : StateBase
{
    public HurtState(Player player) : base(player) {}

    public override void Enter()
    {
        // cancel hurt if invulnerable
        if (Player.isInvulnerable) Player.SetState(Player.states.Idle);

        // handle visuals
        Player.playerAnimationsHandler.animator.SetTrigger("TakingDamage");
        Vector3 hitdirection = Vector3.ProjectOnPlane(Player.hitDirection, Vector3.up).normalized;
        Player.rb.MoveRotation(Quaternion.LookRotation(hitdirection * -1));
        
        // apply knockback
        Player.rb.linearVelocity = Player.hitDirection * Player.knockbackSpeed;
        Player.targetVelocity = Vector3.zero;

        // return to idle after certain time
        Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Idle, Player.knockbackDuration));
    }

    public override void Move(InputAction.CallbackContext ctx, bool ignoreInput = false) 
    {
        if (ignoreInput) Player.movementInput = new Vector2(0, 0);
    }

    public override void Hurt(Vector3 direction) {}
    public override void Sprint(InputAction.CallbackContext ctx) {}
    public override void Dodge(InputAction.CallbackContext ctx) {}
    public override void Jump(InputAction.CallbackContext ctx) {}
    public override void Crouch(InputAction.CallbackContext ctx) {}
    public override void Attack(InputAction.CallbackContext ctx) {}
}