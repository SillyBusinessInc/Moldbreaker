using UnityEngine;

namespace FollowEnemyStates
{
    public class AttackingState : StateBase
    {
        public AttackingState(FollowEnemy followEnemy) : base(followEnemy) {}

        public override void Enter()
        {
            followEnemy.agent.isStopped = true;
        }

        public override void Exit()
        {
            followEnemy.agent.isStopped = false;
        }

        public override void Update()
        {
            if (followEnemy.target == null)
            {
                followEnemy.ChangeState(followEnemy.states.Roaming);
            }

            // Handle cooldown and attack readiness
            if (!followEnemy.canAttack)
            {
                followEnemy.attackTimeElapsed += Time.deltaTime;

                // If cooldown period has passed, allow attacking again
                if (followEnemy.attackTimeElapsed >= followEnemy.attackCooldown)
                {
                    followEnemy.attackTimeElapsed = 0f;
                    followEnemy.toggleCanAttack(true);
                }
                else
                {
                    return;
                }
            }

            // If the player is in range, attempt to face them
            if (IsPlayerInAttackRange())
            {
                FacePlayer();

                if (IsFacingPlayer())
                {
                    Attack();
                }
            }
            else
            {
                // Player out of attack range, switch to following state
                followEnemy.ChangeState(followEnemy.states.Following);
            }
        }

        private void Attack()
        {
            if (!followEnemy.canAttack) return;

            // Proceed with the attack if the player exists and can be damaged
            if (followEnemy.target != null)
            {
                var player = followEnemy.target.root.GetComponent<Player>();
                if (player != null)
                {
                    followEnemy.animator.SetTrigger("TriggerAttackAnimation");
                    player.lastDamageCause = Player.DamageCause.ENEMY;
                    player.OnHit(followEnemy.attackDamage, followEnemy.transform.forward);
                }
            }

            // After attacking, disable attacking until cooldown is over
            followEnemy.toggleCanAttack(false);
        }

        private void FacePlayer()
        {
            if (followEnemy.target == null) return;

            // Get direction to the player
            Vector3 directionToPlayer = (followEnemy.target.position - followEnemy.transform.position).normalized;

            // Calculate the rotation required to face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the player
            followEnemy.transform.rotation = Quaternion.Slerp(
                followEnemy.transform.rotation,
                targetRotation,
                Time.deltaTime * followEnemy.agent.angularSpeed
            );
        }

        private bool IsFacingPlayer()
        {
            if (followEnemy.target == null) return false;

            // Check if the enemy is facing the player (within a certain threshold angle)
            Vector3 directionToPlayer = (followEnemy.target.position - followEnemy.transform.position).normalized;
            float angleToPlayer = Vector3.Angle(followEnemy.transform.forward, directionToPlayer);

            // Allow a small threshold angle to accommodate slight deviations
            return angleToPlayer < 5f;
        }
    }
}
