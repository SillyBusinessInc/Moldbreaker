using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TailAttacks", menuName = "FlipTail")]
public class FlipAttack : TailAttack
{
    public FlipAttack(string Name, float damage) : base(Name, damage) {}

    public override void Start()
    {
        base.Start();
        AudioManager.Instance.PlaySFX("AttackVOX3");
        player.Tail.slamObject.transform.localScale = new Vector3(3, 1, 3);
        player.Tail.slamObject.transform.localScale *=
            player.Tail.tailStatistic.slamObjectSize.GetValue();
        player.Tail.tailDoDamage = player.Tail.tailStatistic.flipTailDamage.GetValue();
        player.Tail.cooldownTime = player.Tail.tailStatistic.flipTailCooldown.GetValue();
        player.Tail.tailDoDamage *= player.playerStatistic.AttackDamageMultiplier.GetValue();
        Animator animator = player.playerAnimationsHandler.animator;
        ClipDuration(animator, duration, "Breadaplus|Bradley_attack2_frontflip");
        animator.speed *= player.playerStatistic.AttackSpeedMultiplier.GetValue();

        player.playerAnimationsHandler.ResetStates();
        player.playerAnimationsHandler.SetInt("AttackType", 2);
        player.playerAnimationsHandler.animator.SetTrigger("IsAttackingTrigger");
    }
    public override IEnumerator SetStateIdle()
    {
        yield return new WaitForSeconds(duration / 2);
        player.Tail.slamObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        player.Tail.slamObject.SetActive(false);
        yield return new WaitForSeconds(duration / 2);
        if (player.isGrounded) player.SetState(player.movementInput.magnitude > 0 ? player.states.Walking : player.states.Idle);
        else player.SetState(player.states.Falling);
    }
}
