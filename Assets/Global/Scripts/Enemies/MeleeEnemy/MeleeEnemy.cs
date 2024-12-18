using UnityEngine;

namespace EnemiesNS
{
    public class MeleeEnemy : EnemyBase
    {
        public enum AttackType
        {
            Stab = 0,
            Slam = 1,
            Slash = 2,
            Slash360 = 3,
            BruteSlash = 4
        }

        public AttackType attackType;

        // check if chosen attack is valid for this enemy
        public virtual bool IsValidAttack(AttackType attackType)
        {
            return attackType == AttackType.Stab ||
                   attackType == AttackType.Slam ||
                   attackType == AttackType.Slash ||
                   attackType == AttackType.Slash360;
        }

        protected override void Start()
        {
            base.Start();
            if (!IsValidAttack(attackType)) Debug.LogWarning($"{attackType} is not valid for this type of enemy", this);
        }
        private bool playerHit = false;

        //
        // called in animations as events
        //
        public override void EnableWeaponHitBox()
        {
            weapon.enabled = true;
        }

        public override void DisableWeaponHitBox()
        {
            weapon.enabled = false;
            playerHit = false;
        }

        public override void PlayerHit(PlayerObject playerObject, int damage)
        {
            if (playerHit) return;
            playerHit = true;
            base.PlayerHit(playerObject, damage);
        }

        protected override void SetupStateMachine()
        {
            states = new MeleeStates(this);
            ChangeState(states.Idle);
        }
    }
}