using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : StateBase
{

    public WalkingState(Player player) : base(player) {}


    public Vector3 debug_hitpos;
    public override void Update()
    {

        Player.playerAnimationsHandler.resetStates();
        Player.playerAnimationsHandler.SetBool("IsRunning", true);
        
        // perform ground check first
        if (!Player.isGrounded) {
            Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Falling, Player.coyoteTime));
        }
        
        // calculate walking direction and speed
        if (Player.GetDirection() != Vector3.zero) Player.currentWalkingPenalty += Player.acceleration * Time.deltaTime;
        else Player.currentWalkingPenalty -= Player.acceleration * Time.deltaTime;
        Player.currentWalkingPenalty = Mathf.Clamp(Player.currentWalkingPenalty, Player.maxWalkingPenalty, 1);

        // apply speed stat
        Vector3 newTargetVelocity = Player.currentWalkingPenalty * Player.playerStatistic.Speed.GetValue() * new Vector3(Player.GetDirection().x, 0, Player.GetDirection().z);

        // add gravity to y velocity
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);

        // slow down on turn
        Vector3 flatLinearVelocity = new(Player.rb.linearVelocity.x, 0 ,Player.rb.linearVelocity.z);
        if (Vector3.Angle(flatLinearVelocity, newTargetVelocity) > 45) Player.rb.linearVelocity *= 0.2f;

        // apply force
        Player.targetVelocity = new Vector3(newTargetVelocity.x, linearY, newTargetVelocity.z);

        // set player to idle if not moving
        if (Player.rb.linearVelocity == Vector3.zero && Player.targetVelocity == Vector3.zero) Player.SetState(Player.states.Idle);
    }
}
