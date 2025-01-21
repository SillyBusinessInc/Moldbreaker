using UnityEngine;

namespace EnemiesNS
{
    public class RangedAttackingState : BaseAttackingState
    {
        private new RangedEnemy enemy;
        private float currentTime;
        private bool canshoot = true;
        private int currentAttack = 0;
        private bool isAttacking = false; // Tracks if currently in attack animation
        public RangedAttackingState(RangedEnemy enemy) : base(enemy) { this.enemy = enemy; }

        public override void Enter()
        {
            base.Enter();
            currentTime = 0;
            currentAttack = 0;
            canshoot = true;
            isAttacking = false; // Ensure attack is not active

            enemy.animator.SetBool("AttackIdle", true);
        }

        public override void Update()
        {
            base.Update();
            if (enemy.distanceToPlayer >= 2)
            {
                FacePlayer();
            }
            if (CheckingInRange()){
                if (canshoot && currentTime > enemy.attackRecoveryTime && CheckingInRange())
                {
                    StartAttack();
                }
                // If attacking, handle the animation progress
                if (isAttacking)
                {
                    HandleAttackProgress();
                    return; // Exit Update while handling attack animation
                }

                // Check if the enemy can attack
                

                // Increment recovery time if not attacking
                if (canshoot)
                {
                    currentTime += Time.deltaTime;
                }

                // Face the player if needed
                
            }
        }

        private void StartAttack()
        {
            currentTime = 0;
            canshoot = false;
            isAttacking = true;

            // Trigger the attack animation
            enemy.animator.SetBool("AttackIdle", false);
            enemy.animator.SetTrigger("AttackStart");
        }

        private void HandleAttackProgress()
        {
            var stateInfo = enemy.animator.GetCurrentAnimatorStateInfo(0);

            string Idle = stateInfo.IsName("attackIdle") ? "attackIdle" : "Unknown";
            // Wait for the animation to reach the bullet firing point
            if (Idle != "attackIdle" && stateInfo.normalizedTime >= 0.35f && stateInfo.normalizedTime < 1f && !enemy.inAttackAnim)
            {
                Attack();
                
                enemy.inAttackAnim = true; // Ensure bullet fires only once
            }

            // Wait for the animation to finish
            if ( stateInfo.normalizedTime >= 1f)
            {
                EndAttack();
            }
        }

        private void EndAttack()
        {
            isAttacking = false; // Reset attack state
            enemy.inAttackAnim = false;

            if (currentAttack < enemy.attacksPerCooldown)
            {
                enemy.animator.SetBool("AttackIdle", true);
            }

            canshoot = true; // Allow for next attack
        }

        public bool CheckingInRange()
        {
            if (!IsWithinAttackRange() || currentAttack >= enemy.attacksPerCooldown)
            {
                enemy.animator.SetBool("AttackIdle", false);
                currentAttack = 5000;
                enemy.inAttackAnim = false;
                CheckState();
                return false;
            }
            return true;
        }


        public override void Exit()
        {
            enemy.animator.SetBool("AttackIdle", false);
            base.Exit();
        }

        protected override void Attack()
        {
            if (CheckingInRange())
            {
                currentAttack += 1;
                GlobalReference.GetReference<AudioManager>().PlaySFX("RangedShot", enemy.transform.position);

                // enemy.animator.SetTrigger("AttackStart");
                GameObject bullet = Object.Instantiate(enemy.bulletPrefab, enemy.bulletSpawnPoint.position, Quaternion.identity);

                // Assign the forward direction of the enemy to the bullet
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.bulletDirection = (GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget.transform.position - enemy.bulletSpawnPoint.position).normalized;
                }

                // Reset the timer and increment the shot count
                enemy.inAttackAnim = true;
                enemy.toggleIsRecovering(true);
                canshoot = true;

            }
        }

    }
}