using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathState : StateBase
{
    private bool isNotDeath;
    private float time;
    public DeathState(Player player) : base(player) { }

    public override void Enter()
    {
        Player.playerAnimationsHandler.animator.SetTrigger("IsDeath");
        time = 0;
        isNotDeath = true;
    }

    public override void Update()
    {
        float linearY = ApplyGravity(Player.rb.linearVelocity.y);
        Player.targetVelocity = new Vector3(0, linearY, 0);

        time += Time.deltaTime;
        if (Player.playerAnimationsHandler.animator.runtimeAnimatorController.animationClips.Where(x => x.name == "Breadaplus|Bradley_death").FirstOrDefault().length*0.8 <= time)
        {
            if (isNotDeath)
            {
                isNotDeath = false;
                SceneManager.LoadScene("Death");
            }
        }
    }

    public override void Move(InputAction.CallbackContext ctx)
    {
    }

    public override void Sprint(InputAction.CallbackContext ctx) { }

    public override void Dodge(InputAction.CallbackContext ctx)
    {
    }

    public override void Jump(InputAction.CallbackContext ctx)
    {
    }

    public override void Glide(InputAction.CallbackContext ctx)
    {
    }

    public override void Crouch(InputAction.CallbackContext ctx) { }

    public override void Attack(InputAction.CallbackContext ctx)
    {
    }
}