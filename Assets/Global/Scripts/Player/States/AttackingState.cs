using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Rendering;
public class AttackingState : StateBase
{
    public AttackingState(Player player) : base(player) { }

    public void IncreaseIndex()
    {
        Player.Tail.attackIndex =
            Player.Tail.attackIndex >= Player.Tail.currentTail.currentCombo.Count - 1
                ? 0
                : ++Player.Tail.attackIndex;
    }

    public override void Enter()
    {
        var tail = Player.Tail.currentTail;
        if (Player.AirComboDone || !(Player.Tail.activeCooldownTime >= Player.Tail.cooldownTime) || tail.currentCombo.Count == 0)
        {
            if (Player.isGrounded) Player.SetState(Player.movementInput.magnitude > 0 ? Player.states.Walking : Player.states.Idle);
            else Player.SetState(Player.states.Falling);
            return;
        }
        Player.targetVelocity *= 0;
        Player.rb.linearVelocity *= 0;
        Player.Tail.activeCooldownTime = 0.0f;
        Player.Tail.currentTail.currentCombo = Player.isGrounded ? Player.Tail.currentTail.groundCombo : Player.Tail.currentTail.airCombo;
        Player.AirComboDone = !Player.isGrounded && Player.Tail.attackIndex >= Player.Tail.currentTail.airCombo.Count - 1;
        var currentCombo = tail.currentCombo[Player.Tail.attackIndex];
        currentCombo.Start();
        Player.StartCoroutine(currentCombo.SetStateIdle());
        IncreaseIndex();
    }

    public override void Exit()
    {
        Player.playerAnimationsHandler.animator.speed = 1.0f;
        Player.Tail.tailCanDoDamage = false;
        Player.collidersEnemy.Clear();
    }

    public override void Attack(InputAction.CallbackContext ctx) { }

    public override void Move(InputAction.CallbackContext ctx)
    {
        Player.movementInput = ctx.ReadValue<Vector2>();
    }

    public override void Sprint(InputAction.CallbackContext ctx) { }

    public override void Dodge(InputAction.CallbackContext ctx) { }

    public override void Jump(InputAction.CallbackContext ctx) { }

    public override void Glide(InputAction.CallbackContext ctx) { }

    public override void Crouch(InputAction.CallbackContext ctx) { }

}
