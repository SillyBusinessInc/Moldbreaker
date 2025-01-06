using UnityEngine;
using UnityEngine.InputSystem;

public class HurtState : StateBase
{
    private float activeKnockbackDuration;

    public HurtState(Player player) : base(player) { }
    public override void Enter()
    {
        if (Player.isInvulnerable) Player.SetState(Player.states.Idle);
        Player.playerAnimationsHandler.animator.SetTrigger("TakingDamage");
        activeKnockbackDuration = 0.0f;
        Vector3 hitdirection = Vector3.ProjectOnPlane(Player.hitDirection, Vector3.up).normalized;
        Player.rb.MoveRotation(Quaternion.LookRotation(hitdirection * -1));
    }

    public override void Update()
    {
        if (activeKnockbackDuration >= Player.knockbackDuration)
        {
            Player.SetState(Player.states.Idle);
        }
        activeKnockbackDuration += Time.deltaTime;
        Player.rb.linearVelocity = Player.hitDirection * Player.knockbackSpeed;
    }

    public override void Hurt(Vector3 direction) { }
    public override void Move(InputAction.CallbackContext ctx) { }

    public override void Sprint(InputAction.CallbackContext ctx) { }

    public override void Dodge(InputAction.CallbackContext ctx) { }

    public override void Jump(InputAction.CallbackContext ctx) { }

    public override void Glide(InputAction.CallbackContext ctx) { }

    public override void Crouch(InputAction.CallbackContext ctx) { }

    public override void Attack(InputAction.CallbackContext ctx) { }
}