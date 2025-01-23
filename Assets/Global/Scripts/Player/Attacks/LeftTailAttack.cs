using UnityEngine;

[CreateAssetMenu(fileName = "TailAttacks", menuName = "LeftTail")]
public class LeftTailAttack : TailAttack
{
    public LeftTailAttack(string Name, float damage, float cooldown) : base(Name, damage) {}

    public override void Start()
    {
        base.Start();
        GlobalReference.GetReference<AudioManager>().PlaySFX("AttackVOX1");
        player.Tail.tailCanDoDamage = true;
        player.Tail.tailDoDamage = player.Tail.tailStatistic.leftTailDamage.GetValue();
        player.Tail.tailDoDamage *= player.playerStatistic.AttackDamageMultiplier.GetValue();
        player.Tail.cooldownTime = player.Tail.tailStatistic.leftTailCooldown.GetValue();
        Animator animator = player.playerAnimationsHandler.animator;
        ClipDuration(animator, duration, "Breadaplus|Bradley_attack1_L");
        animator.speed *= player.Tail.tailStatistic.increaseTailSpeed.GetValue();
        animator.speed *= player.playerStatistic.AttackSpeedMultiplier.GetValue();

        player.playerAnimationsHandler.ResetStates();
        player.playerAnimationsHandler.SetInt("AttackType", 1);
        player.playerAnimationsHandler.animator.SetTrigger("IsAttackingTrigger");
        player.Tail.CanShowFeedback = false;
    }

}
