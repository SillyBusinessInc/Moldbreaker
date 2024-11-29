using UnityEngine;

namespace EnemiesNS
{
    public class MeleeRoamingState : BaseRoamingState
    {
        public MeleeRoamingState(MeleeEnemy enemy) : base(enemy) { }

        public override void Enter()
        {
            Debug.Log("Triggering walk bool");
            enemy.animator.SetBool("Walk", true);
            base.Enter();
        }
        public override void Exit()
        {
            enemy.animator.SetBool("Walk", false);
            base.Exit();
        }

    }
}
