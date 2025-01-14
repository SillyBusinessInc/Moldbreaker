using UnityEngine;
using UnityEngine.InputSystem;

public class FallingState : StateBase
{
    public FallingState(Player player) : base(player) {}

    public override void Enter()
    {
        Player.playerAnimationsHandler.SetBool("IsFallingDown", false);
    }

    public override void Update()
    {
        // check if player is done with jump animation

        // AnimatorStateInfo stateInfo = Player.playerAnimationsHandler.animator.GetCurrentAnimatorStateInfo(0); // Layer 0 is usually the default layer
        // if (stateInfo.IsName("Breadaplus|Bradley_jump_from_ground 0")) // Replace "AnimationName" with your animation's name
        // {
        //     if (stateInfo.normalizedTime >= 1.0f && !Player.playerAnimationsHandler.animator.IsInTransition(0))
        //     {
        //         Debug.Log("Animation finished!");
        //         Player.playerAnimationsHandler.SetBool("IsJumpingBool",false);
        //     }
        // }
        
        //set isfallowdown true if velocity is falling downwards only downwards 
        if (Player.rb.linearVelocity.y < -1)
        {
            Player.playerAnimationsHandler.SetBool("IsFallingDown", true);
            // Player.playerAnimationsHandler.animator.ResetTrigger("IsLanding");
        }
        else if (Player.rb.linearVelocity.y > 0)
        {
            Player.playerAnimationsHandler.SetBool("IsFallingDown", false);
        }

        // add gravity to y velocity
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);

        // apply horizontal momentum based on input
        Vector3 newTargetVelocity = Player.GetDirection() * (Player.playerStatistic.Speed.GetValue() * Player.airBorneMovementFactor);
        Player.targetVelocity = new(newTargetVelocity.x, linearY, newTargetVelocity.z);

        // change state on ground
        // if (Player.isGrounded) Player.SetState(Player.movementInput.magnitude > 0 ? Player.states.Walking : Player.states.Idle);
        if (Player.isGrounded && Player.movementInput.sqrMagnitude == 0) Player.SetState(Player.states.Idle);
        else if (Player.isGrounded) {
            Player.SetState(Player.states.Walking);
        }
    }

    public override void Jump(InputAction.CallbackContext ctx)
    {
        // air jump animation 
        if (ctx.started && Player.playerStatistic.DoubleJumpsCount.GetValueInt() > Player.currentJumps)
        {
            Player.playerAnimationsHandler.animator.SetTrigger("IsDoubleJumping");
            Player.currentJumps += 1;
            Player.isHoldingJump = true;
            Player.SetState(Player.states.Jumping);
        }
        if (ctx.canceled) 
        {
            Player.isHoldingJump = false;
        }
    }

    public override void Glide(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && Player.rb.linearVelocity.y < 0)
        {
            Player.SetState(Player.states.Gliding);
        }
    }
}
