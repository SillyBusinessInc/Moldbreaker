using UnityEngine;

namespace EnemiesNS
{
    public class MeleeEnemy : EnemyBase
    {
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

        public override void PlayerHit(PlayerObject playerObject, int damage, Vector3 knockback)
        {
            if (playerHit) return;
            playerHit = true;
            base.PlayerHit(playerObject, damage, knockback);
        }

        protected override void SetupStateMachine()
        {
            states = new MeleeStates(this);
            ChangeState(states.Idle);
        }
    }
}