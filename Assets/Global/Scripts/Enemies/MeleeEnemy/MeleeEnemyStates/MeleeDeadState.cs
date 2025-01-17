namespace EnemiesNS
{
    public class MeleeDeadState : BaseDeadState
    {
        public MeleeDeadState(MeleeEnemy enemy) : base(enemy) {}

        public override void Enter()
        {
            base.Enter();
            enemy.animator.SetInteger("DeathReason", 1);
            enemy.animator.SetTrigger("PlayDeath");
        }
    }
}
