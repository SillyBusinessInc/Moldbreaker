using UnityEngine;

namespace EnemiesNS
{
    public class RangedDeadState : BaseDeadState
    {
        public RangedDeadState(RangedEnemy enemy) : base(enemy) { }

        public override void Enter()
        {
            base.Enter();
            enemy.animator.SetInteger("DeathReason", 1);
            enemy.animator.SetTrigger("PlayDeath");

        }

    }
}
