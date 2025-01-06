using UnityEngine;

namespace EnemiesNS
{
    public class RangedIdleState : BaseIdleState
    {
        public RangedIdleState(RangedEnemy enemy) : base(enemy) { }
        public override void Enter()
        {
            int i = Random.Range(0, 2);
            enemy.animator.SetInteger("Idle_var", i);
            enemy.animator.SetBool("Idle", true);
            base.Enter();
        }
        public override void Exit()
        {
            enemy.animator.SetBool("Idle", false);
            base.Exit();
        }

    }
}
