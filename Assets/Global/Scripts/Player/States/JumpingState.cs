using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingState : StateBase
{
    public JumpingState(Player player) : base(player) { }

    public override void Enter()
    {

        Player.playerAnimationsHandler.animator.ResetTrigger("IsLanding");
        Player.playerAnimationsHandler.SetBool("IsFallingDown", false);
        Player.playerAnimationsHandler.SetBool("IsJumpingBool", true);
        Player.playerAnimationsHandler.animator.SetTrigger("IsJumping");

        // play particleSystem
        Player.particleSystemJump.Play();

        // add force upwards
        Player.rb.linearVelocity = new Vector3(Player.rb.linearVelocity.x, Player.playerStatistic.JumpForce.GetValue(), Player.rb.linearVelocity.z);
        Player.targetVelocity = Player.rb.linearVelocity;

        // change state to falling after a bit to give the player some time to reach intended height
        AudioManager.Instance.PlaySFX("JumpSFX");
        Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Falling, Player.maxJumpHoldTime, true));
    }

    public override void Update()
    {
        // force state change if player let's go of jump button early
        if (!Player.isHoldingJump) Player.SetState(Player.states.Falling);
    }

    public override void Move(InputAction.CallbackContext ctx)
    {
        Player.movementInput = ctx.ReadValue<Vector2>();
    }
}
