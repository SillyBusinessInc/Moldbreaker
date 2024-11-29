using UnityEngine;

namespace EnemiesNS
{
    public class MeleeIdleState : BaseIdleState
    {
        public MeleeIdleState(MeleeEnemy enemy) : base(enemy) { }
        public override void Enter()
        {
            int i = Random.Range(0, 2);
            Debug.Log("Triggering idle bool");
            enemy.animator.SetInteger("Idle_var", i);
            enemy.animator.SetBool("Idle", true);
            base.Enter();
        }
        public override void Exit()
        {
            enemy.animator.SetBool("idle", false);
            base.Exit();
        }

    }
}
