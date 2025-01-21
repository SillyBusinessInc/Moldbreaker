using UnityEngine;

namespace EnemiesNS
{
    public class BaseDeadState : StateBase
    {
        public BaseDeadState(EnemyBase enemy) : base(enemy) {}

        public override void Update()
        {
            enemy.GetComponent<BoxCollider>().enabled = false;
            return;
        }
    }
}