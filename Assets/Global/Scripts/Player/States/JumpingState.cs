using UnityEngine;

public class JumpingState : StateBase
{
    public JumpingState(Player player) : base(player) {}

    public override void Enter()
    {
        // add force upwards
        Player.rb.linearVelocity = new Vector3(Player.rb.linearVelocity.x, Player.playerStatistic.JumpForce.GetValue(), Player.rb.linearVelocity.z);
        Player.targetVelocity = Player.rb.linearVelocity;

        // change state to falling after a bit to give the player some time to reach intended height
        Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Falling, Player.maxJumpHoldTime, true));
    }

    public override void Update()
    {
        // force state change if player let's go of jump button early
        if (!Player.isHoldingJump) Player.SetState(Player.states.Falling);
    }
}
