using UnityEngine;
using UnityEngine.AI;

namespace EnemiesNS
{
    public class RangedChasingState : BaseChasingState
    {
        private bool isMovingToRandomPosition = false;
        private new RangedEnemy enemy;
        public RangedChasingState(RangedEnemy enemy) : base(enemy) { this.enemy = enemy; }
        private float timeSinceLastRandomMove = 0f;
        private float randomMoveCooldown = 2f; // Cooldown time in seconds
        private bool doneWalking = false;

        public override void Enter()
        {
            doneWalking = false;
            enemy.animator.SetBool("Walk", true);
            base.Enter();
        }

        public override void Exit()
        {
            enemy.animator.SetBool("Walk", false);
            base.Exit();
        }
        public override void Update()
        {
            enemy.animator.SetFloat("WalkingSpeed", enemy.agent.velocity.magnitude);
            CalculateDistanceToPlayer(); // Decide if needed every frame

            if(!IsWithinAttackRange()){
                doneWalking = false;
            }
            if (!enemy.isWaiting)
            {
                if (enemy.agent.isStopped)
                    enemy.FreezeMovement(false);

                if (isMovingToRandomPosition)
                {
                    // Check if the enemy has reached the random position
                    if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
                    {
                        isMovingToRandomPosition = false; // Reset the flag
                        timeSinceLastRandomMove = Time.time; // Start cooldown
                        CheckState(); // Allow state change here
                    }
                }
                else if (IsWithinAttackRange() && !doneWalking)
                {
                    doneWalking = true;
                    // Prevent random movement if cooldown hasn't expired
                    if (!isMovingToRandomPosition && Time.time >= timeSinceLastRandomMove + randomMoveCooldown)
                    {
                        Vector3 randomDestination = GetRandomNavMeshPosition(enemy.transform.position, enemy.moveRadiusAfterAttacking);
                        enemy.agent.SetDestination(randomDestination);
                        isMovingToRandomPosition = true; // Set the flag
                    }
                } 
                else if (!doneWalking)
                {
                    doneWalking = true;
                    // Chase the player
                    enemy.agent.SetDestination(enemy.target.transform.position);
                }
            }
            // Update the walking speed for animation


            if (!isMovingToRandomPosition && IsWithinAttackRange())
            {
                enemy.FreezeMovement(true);
                enemy.isRecovering = false;
                enemy.canAttack = true;
                enemy.inAttackAnim = false;
                CheckState();
            }
        }

        private Vector3 GetRandomNavMeshPosition(Vector3 origin, float distance)
        {
            // Generate a random direction within a sphere
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection.y = 0; // Ensure the position stays on the horizontal plane
            randomDirection += origin;

            // Sample the position on the NavMesh
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, NavMesh.AllAreas))
            {
                return navHit.position; // Return a valid NavMesh position
            }

            return origin; // Fallback to the original position if no valid point is found
        }
        // Draw Gizmos to visualize the random destination
        private void OnDrawGizmosSelected()
        {
            if (isMovingToRandomPosition && enemy != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(enemy.agent.destination, 0.5f); // Visualize the random destination
            }
        }

    }
}

