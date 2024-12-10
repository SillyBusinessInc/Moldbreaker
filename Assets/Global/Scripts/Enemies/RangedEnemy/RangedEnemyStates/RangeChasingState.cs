using UnityEngine;
using UnityEngine.AI;

namespace EnemiesNS
{
    public class RangeChasingState : BaseChasingState
    {
        private bool isMovingToRandomPosition = false;
        public RangeChasingState(RangedEnemy enemy) : base(enemy) { }

        public override void Enter()
        {
            enemy.animator.SetBool("Walk", true);
            base.Enter();
        }

        public override void Exit()
        {
            enemy.animator.SetBool("Walk", false);
            base.Exit();
        }


        private float timeSinceLastRandomMove = 0f;
        private float randomMoveCooldown = 2f; // Cooldown time in seconds

        public override void Update()
        {
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
                        Debug.Log("Reached Random Destination.");
                        CheckState(); // Allow state change here
                    }
                }
                else if (IsWithinAttackRange())
                {
                    // Prevent random movement if cooldown hasn't expired
                    if (!isMovingToRandomPosition && Time.time >= timeSinceLastRandomMove + randomMoveCooldown)
                    {
                        Vector3 randomDestination = GetRandomNavMeshPosition(enemy.transform.position, enemy.attackRange / 2);
                        Debug.Log($"Random Destination: {randomDestination}");
                        enemy.agent.SetDestination(randomDestination);
                        isMovingToRandomPosition = true; // Set the flag
                    }
                }
                else
                {
                    // Chase the player
                    enemy.agent.SetDestination(enemy.target.transform.position);
                }
            }

            // Update the walking speed for animation
            enemy.animator.SetFloat("WalkingSpeed", enemy.agent.velocity.magnitude);
            CalculateDistanceToPlayer(); // Decide if needed every frame

            // Only check state if the enemy is not moving to a random position
            if (!isMovingToRandomPosition)
            {
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
