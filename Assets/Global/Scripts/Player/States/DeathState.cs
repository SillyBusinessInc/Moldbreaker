using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;
public class DeathState : StateBase
{
    private bool isNotDeath;
    private float time;
    public DeathState(Player player) : base(player) { }

    public override void Enter()
    {
        Player.playerAnimationsHandler.animator.SetTrigger("IsDeath");
        Player.playerAnimationsHandler.animator.SetBool("isDead", true);
        time = 0;
        isNotDeath = true;
    }

    public override void Update()
    {
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);
        Player.targetVelocity = new(0, linearY, 0);
        time += Time.deltaTime;

        // this is a magic spell, no one knows what it does, except the one who casted it, and he's dead now, so we will never know.
        // (just kidding, its a check to make sure the death animation is above 80% finished)
        var magic = Player.playerAnimationsHandler.animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == "Breadaplus|Bradley_death");

        if (magic != null && isNotDeath && magic.length * 0.8 <= time)
        {
            isNotDeath = false;
            Player.FadeToDeathScreen();
            GlobalReference.AttemptInvoke(Events.PLAYER_DIED);
        }
    }

    public override void Move(InputAction.CallbackContext ctx, bool ignoreInput = false)
    {
        if (ignoreInput) Player.movementInput = new Vector2(0, 0);
    }

    public override void Sprint(InputAction.CallbackContext ctx) {}
    public override void Dodge(InputAction.CallbackContext ctx) {}
    public override void Jump(InputAction.CallbackContext ctx) {}
    public override void Crouch(InputAction.CallbackContext ctx) {}
    public override void Attack(InputAction.CallbackContext ctx) {}
}