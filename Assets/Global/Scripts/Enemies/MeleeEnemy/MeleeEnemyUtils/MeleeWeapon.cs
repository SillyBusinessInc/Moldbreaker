using UnityEngine;

namespace EnemiesNS
{
    public class MeleeWeapon : MonoBehaviour
    {
        EnemyBase enemy;
        void Start()
        {
            enemy = this.GetComponentInParent<EnemyBase>();
        }

        void OnTriggerEnter(Collider hit)
        {
            if (!enemy) return;
            hit.TryGetComponent(out PlayerObject player);
            if (!player) return;
            Vector3 myPosition = transform.position;

            // Get the other Collider's position
            Vector3 otherPosition = hit.ClosestPoint(transform.position); // ClosestPoint gives you the nearest point on the collider surface.

            // Compute a pseudo-normal by subtracting positions
            Vector3 normal = (myPosition - otherPosition).normalized;
            enemy.PlayerHit(player, enemy.attackDamage, normal);
        }
    }
}