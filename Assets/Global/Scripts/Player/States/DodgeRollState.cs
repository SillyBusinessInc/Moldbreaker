using UnityEngine;
using UnityEngine.InputSystem;

public class DodgeRollState : StateBase
{
    private float timer;

    public DodgeRollState(Player player) : base(player) { }

    public override void Enter()
    {
        // return if on cooldown
        if (Time.time < Player.timeLastDodge + Player.playerStatistic.DodgeCooldown.GetValue())
        {
            ExitDodge();
            return;
        }
        // play particleSystem
        Player.particleSystemDash.Play();
        System.Random random = new System.Random();
        int chance = random.Next(1, 101);
        if (chance == 1)
        {
            GlobalReference.GetReference<AudioManager>().PlaySFX("fart");
        }
        else
        {
            GlobalReference.GetReference<AudioManager>().PlaySFX("Dash");
        }        
        Player.timeLastDodge = Time.time;
        Player.playerAnimationsHandler.animator.SetTrigger("IsDodgeRoll");

        // find direction
        Vector3 dodgeDirection = Player.GetDirection();

        // set timer after which we can change state
        timer = Player.dodgeRollDuration;
        Player.canDodgeRoll = false;

        // force forward position if the player isn't directly giving us a direction
        if (dodgeDirection == Vector3.zero) dodgeDirection = Player.rb.transform.forward.normalized;

        // apply force
        Player.rb.linearVelocity = dodgeDirection * Player.dodgeRollSpeed;
        Player.targetVelocity = new(Player.targetVelocity.x, 0, Player.targetVelocity.z);

        // player.animator.SetTrigger("DodgeRoll");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && Player.playerStatistic.DoubleJumpsCount.GetValueInt() > Player.currentJumps)
        {
            // this will normalize the momentum preventing the momentum to be used to get an extra high jump
            Player.rb.linearVelocity = Player.rb.linearVelocity.normalized;

            Player.currentJumps += 1;
            Player.isHoldingJump = true;
            Player.SetState(Player.states.Jumping);
        }
        if (ctx.canceled) 
        {
            Player.isHoldingJump = false;
        }
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) ExitDodge();
    }

    public void ExitDodge() {
        if (Player.isGrounded) Player.SetState(Player.movementInput.magnitude > 0 ? Player.states.Walking : Player.states.Idle);
        else Player.SetState(Player.states.Falling);
    }
}