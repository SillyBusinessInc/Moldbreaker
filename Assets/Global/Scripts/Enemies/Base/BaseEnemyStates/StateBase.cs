using UnityEngine;

namespace EnemiesNS
{
    public abstract class StateBase
    {
        protected EnemyBase enemy;
        protected StateBase(EnemyBase enemy_)
        {
            enemy = enemy_;
        }

        public virtual void Enter() {}

        public virtual void Exit()
        {
            enemy.FreezeMovement(false);
        }

        public virtual void Update()
        {
            
            enemy.animator.SetFloat("WalkingSpeed", enemy.agent.velocity.magnitude);
            CalculateDistanceToPlayer(); // do we want to calculate on every frame?
            CheckState();
        }
        
        public virtual void FixedUpdate() {}

        //
        // Add methods here that need to be accessed by multiple different states
        //

        protected void CheckState()
        {
            // dead or alive check
            if (enemy.currentState == enemy.states.Dead) return;
            // check to see if enemy is still recovering from attacking
            if (enemy.isRecovering || enemy.inAttackAnim) return;
            // attack
            if (enemy.currentState != enemy.states.Attacking && enemy.canAttack && IsWithinAttackRange())
            {
                enemy.ChangeState(enemy.states.Attacking);
                return;
            }
            // chase
            if (enemy.isWaiting) return;
            if (enemy.currentState == enemy.states.Chasing) return;
            if (enemy.currentState != enemy.states.Chasing && (enemy.isChasing || IsWithinChaseRange()))
            {
                enemy.ChangeState(enemy.states.Chasing);
                return;
            }
            // roaming
            if (enemy.currentState != enemy.states.Roaming && !enemy.isIdling)
            {
                enemy.ChangeState(enemy.states.Roaming);
                return;
            }
            // idle           
            if (enemy.currentState != enemy.states.Idle && enemy.agent.remainingDistance < 1)
            {
                enemy.ChangeState(enemy.states.Idle);
                return;
            }
        }

        protected void CalculateDistanceToPlayer()
        {
            enemy.distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.target.position);
        }

        protected bool IsWithinChaseRange()
        {
            //early return for when already chasing
            if (enemy.isChasing) return true;
            return enemy.distanceToPlayer <= enemy.chaseRange;
        }

        protected bool IsWithinAttackRange()
        {
            return enemy.distanceToPlayer <= enemy.attackRange;
        }
    }
}