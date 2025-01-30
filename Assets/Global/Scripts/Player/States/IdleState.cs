using UnityEngine;

public class IdleState : StateBase
{
    private float idleSpecialTimeRemaining;

    public IdleState(Player player) : base(player) {}

    public override void Enter()
    {
        idleSpecialTimeRemaining = Random.Range(Player.MinIdleAnimTime, Player.maxIdleAnimTime);
        Player.playerAnimationsHandler.ResetStates();
    }

    public override void Update()
    {
        // add gravity to y velocity
        var linearY = ApplyGravity(Player.rb.linearVelocity.y);
        Player.targetVelocity = new Vector3(0, linearY, 0);

        if (!Player.isGrounded) Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Falling, Player.coyoteTime));

        idleSpecialTimeRemaining -= Time.deltaTime;
        if (!(idleSpecialTimeRemaining <= 0)) return;
        
        Player.playerAnimationsHandler.SetInt("IdleSpecialType", Random.Range(1, 3));
        Player.playerAnimationsHandler.animator.SetTrigger("IdleSpecial");
        GlobalReference.GetReference<AudioManager>().PlaySFX("BradleyIdleSpecial");
        idleSpecialTimeRemaining = Random.Range(Player.MinIdleAnimTime, Player.maxIdleAnimTime);
    }
}
