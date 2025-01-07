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
        var forwardDirection = Vector3.ProjectOnPlane(GlobalReference.GetReference<PlayerReference>().PlayerCamera.transform.forward, Vector3.up).normalized;
        Player.rb.MoveRotation(Quaternion.LookRotation(forwardDirection));
        // play particleSystem
        Player.particleSystemDash.Play();
        
        Player.timeLastDodge = Time.time;
        Player.playerAnimationsHandler.SetBool("Dodgerolling", true);

        // set timer after which we can change state
        timer = Player.dodgeRollDuration;
        Player.canDodgeRoll = false;

        // apply force
        Player.rb.linearVelocity = forwardDirection * Player.dodgeRollSpeed;
        Player.targetVelocity = new(Player.targetVelocity.x, 0, Player.targetVelocity.z);
    }

    public override void Exit()
    {
        base.Exit();
        Player.playerAnimationsHandler.SetBool("Dodgerolling", false);
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

        if (timer <= 0)
        {
            ExitDodge();
        }
    }

    public void ExitDodge() {
        if (Player.isGrounded) Player.SetState(Player.movementInput.magnitude > 0 ? Player.states.Walking : Player.states.Idle);
        else Player.SetState(Player.states.Falling);
    }
}