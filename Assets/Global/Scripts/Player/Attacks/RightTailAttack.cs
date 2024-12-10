using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TailAttacks", menuName = "RightTail")]
public class RightTailAttack : TailAttack
{
    public RightTailAttack(string Name, float damage, float cooldown)
        : base(Name, damage, cooldown) { }

    public override void Start()
    {
        base.Start();
        player.Tail.tailCanDoDamage = true;
        player.Tail.tailDoDamage = player.Tail.tailStatistic.rightTailDamage.GetValue();
        player.Tail.tailDoDamage *= player.playerStatistic.AttackDamageMultiplier.GetValue();
        player.Tail.cooldownTime = player.Tail.tailStatistic.rightTailCooldown.GetValue();
        Animator animator = player.playerAnimationsHandler.animator;
        ClipDuration(animator, duration, "Breadaplus|Bradley_attack1_R");
        animator.speed *= player.Tail.tailStatistic.increaseTailSpeed.GetValue();
        animator.speed *= player.playerStatistic.AttackSpeedMultiplier.GetValue();

        player.playerAnimationsHandler.resetStates();
        player.playerAnimationsHandler.SetInt("AttackType", 0);
        player.playerAnimationsHandler.animator.SetTrigger("IsAttackingTrigger");
    }
}
