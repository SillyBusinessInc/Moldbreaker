using UnityEngine;
using UnityEngine.AI;

namespace EnemiesNS
{
    public class BaseRoamingState : StateBase
    {
        public BaseRoamingState(EnemyBase enemy) : base(enemy) { }

        public override void Enter()
        {
            base.Enter();
            if (enemy.particleSystemWalk) enemy.particleSystemWalk.Play();
            enemy.agent.speed = enemy.roamingSpeed;
            enemy.agent.acceleration = enemy.roamingAcceleration;
            // new Destination
            enemy.roamDestination = GetDestination();
            enemy.agent.SetDestination(enemy.roamDestination);
        }

        public override void Exit()
        {
            if (enemy.particleSystemWalk) enemy.particleSystemWalk.Stop();
            base.Exit();
        }

        public override void Update()
        {
            if (enemy.isPathBlocked && enemy.agent.remainingDistance < 1f) enemy.toggleIsPathblocked(false);
            base.Update();
        }

        private Vector3 GetDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * enemy.roamRange;
            randomDirection += enemy.spawnPos;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, enemy.roamRange, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return enemy.spawnPos;
        }
    }
}
