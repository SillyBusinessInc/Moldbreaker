namespace EnemiesNS
{
    public class RangedStated : BaseStates
    {
        public RangedStated(RangedEnemy enemy) : base(enemy) { }

        // Override the creation methods to return Melee-specific states
        protected override BaseIdleState CreateIdleState(EnemyBase enemy) => new RangedIdleState((RangedEnemy)enemy);
        protected override BaseRoamingState CreateRoamingState(EnemyBase enemy) => new RangedRoamingState((RangedEnemy)enemy);
        protected override BaseChasingState CreateChasingState(EnemyBase enemy) => new RangedChasingState((RangedEnemy)enemy);
        protected override BaseAttackingState CreateAttackingState(EnemyBase enemy) => new RangedAttackingState((RangedEnemy)enemy);
        protected override BaseDeadState CreateDeadState(EnemyBase enemy) => new RangedDeadState((RangedEnemy)enemy);

        // New properties to provide type-specific access if needed
        public new RangedIdleState Idle => (RangedIdleState)base.Idle;
        public new RangedRoamingState Roaming => (RangedRoamingState)base.Roaming;
        public new RangedChasingState Chasing => (RangedChasingState)base.Chasing;
        public new RangedAttackingState Attacking => (RangedAttackingState)base.Attacking;
        public new RangedDeadState Dead => (RangedDeadState)base.Dead;
    }
}