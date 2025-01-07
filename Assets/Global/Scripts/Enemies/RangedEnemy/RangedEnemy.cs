
using UnityEngine;
namespace EnemiesNS
{
    public class RangedEnemy : EnemyBase
    {
        public Transform bulletSpawnPoint; 
        public GameObject bulletPrefab;
        [Range(0f, 100f)]
        public float moveRadiusAfterAttacking = 5f;
        protected override void SetupStateMachine()
        {
            states = new RangedStated(this);
            ChangeState(states.Idle);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, moveRadiusAfterAttacking);
        }

    }
}