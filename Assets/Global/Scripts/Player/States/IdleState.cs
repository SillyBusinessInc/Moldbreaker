using UnityEngine;


public class IdleState : StateBase
{
    public float time = 20f;
    private float currentTime;

    public IdleState(Player player) : base(player) {}

    public override void Enter()
    {
        currentTime = time;
        Player.playerAnimationsHandler.resetStates();
    }

    public override void Update()
    {
        //was here before need to check this

        // Player.playerAnimationsHandler.SetBool("IsRunning", false);
        // Player.playerAnimationsHandler.resetStates();

        // add gravity to y velocity
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);
        Player.targetVelocity = new Vector3(0, linearY, 0);

        if (!Player.isGrounded) Player.activeCoroutine = Player.StartCoroutine(Player.SetStateAfter(Player.states.Falling, Player.coyoteTime));

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {

            Player.playerAnimationsHandler.SetInt("IdleSpecialType", Random.Range(1, 3));
            Player.playerAnimationsHandler.animator.SetTrigger("IdleSpecial");
            currentTime = time;
        }
    }
    
}
