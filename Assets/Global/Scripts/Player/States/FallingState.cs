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
        
        //set isfallowdown true if velocity is falling downwards only downwards 
        if (Player.rb.linearVelocity.y < -1)
        {
            Player.playerAnimationsHandler.SetBool("IsFallingDown", true);
        }
        else if (Player.rb.linearVelocity.y > 0)
        {
            Player.playerAnimationsHandler.SetBool("IsFallingDown", false);
        }else{
            Player.playerAnimationsHandler.animator.SetTrigger("IsLanding");
        }

        // add gravity to y velocity
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);

        // apply horizontal momentum based on input
        Vector3 newTargetVelocity = Player.GetDirection() * (Player.playerStatistic.Speed.GetValue() * Player.airBorneMovementFactor);
        Player.targetVelocity = new(newTargetVelocity.x, linearY, newTargetVelocity.z);

        // change state on ground
        if (Player.isGrounded && Player.movementInput.sqrMagnitude == 0) Player.SetState(Player.states.Idle);
        else if (Player.isGrounded) Player.SetState(Player.states.Walking);
    }

    public override void Jump(InputAction.CallbackContext ctx)
    {
        // air jump animation 
        if (ctx.started && Player.playerStatistic.DoubleJumpsCount.GetValueInt() > Player.currentJumps)
        {
            Player.currentJumps += 1;
            Player.isHoldingJump = true;
            Player.SetState(Player.states.Jumping);
        }
        if (ctx.canceled) 
        {
            Player.isHoldingJump = false;
        }
    }
}
