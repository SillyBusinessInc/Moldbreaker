namespace EnemiesNS
{
    public class BruteEnemy : MeleeEnemy
    {
        public override bool IsValidAttack(AttackType attackType)
        {
            return attackType == AttackType.Stab ||
                   attackType == AttackType.Slam ||
                   attackType == AttackType.Slash ||
                   attackType == AttackType.Slash360 ||
                   attackType == AttackType.BruteSlash;
        }
    }
}
