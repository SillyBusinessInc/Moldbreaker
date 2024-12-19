using UnityEngine;

namespace EnemiesNS
{
    public class MeleeAttackingState : BaseAttackingState
    {
        public MeleeAttackingState(MeleeEnemy enemy) : base(enemy) { }

        public override void Enter()
        {
            if (enemy is MeleeEnemy meleeEnemy)
            {
                if (meleeEnemy.IsValidAttack(meleeEnemy.attackType))
                {
                    meleeEnemy.animator.SetInteger("Attack_var", (int)meleeEnemy.attackType);
                    meleeEnemy.animator.SetBool("Attack", true);
                }
                else
                {
                    Debug.LogWarning($"{meleeEnemy.attackType} is not valid for this type of enemy", meleeEnemy);
                }
            }
        }

        public override void Exit()
        {
            enemy.DisableWeaponHitBox();
            enemy.animator.SetBool("Attack", false);
            base.Exit();
        }

        protected override void Attack()
        {
            if (enemy is MeleeEnemy meleeEnemy)
            {
                if (meleeEnemy.IsValidAttack(meleeEnemy.attackType)) enemy.animator.SetTrigger("PlayAttack");
                if (IsWithinAttackRange() && canAttack())
                {
                    FacePlayer();
                    if (IsFacingPlayer()) Attack();
                }
                base.Attack();
            }
        }
    }
}