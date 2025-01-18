using UnityEngine;

namespace EnemiesNS
{


    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed = 10f;
        public float bulletLifeTime = 2f;
        public int bulletDamage = 1;
        public int bulletEnemyDamage = 5;
        public Vector3 bulletDirection;
        [Tooltip("The amount of knockback this enemy's attacks will apply")]
        [SerializeField]
        [Range(0f, 100f)]
        public float attackKnockback = 2f;
        public GameObject impactVFX;

        void Start()
        {
            Shot();
        }

        void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.layer == 7 || hit.gameObject.layer == 9)
            {
                PlayerHit(bulletDamage);
            }
            else if (hit.gameObject.CompareTag("Enemy"))
            {
                // Damage enemy
                var enemy = hit.gameObject.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.OnHit(bulletEnemyDamage);
                }
            }


            AudioManager.Instance.PlaySFX("ShotExplosion", transform.position);
            PlayImpactVFX();
            Destroy(gameObject);
        }

        private void PlayerHit(int damage)
        {
            Player player = GlobalReference.GetReference<PlayerReference>().Player;
            if (!player) return;
            player.lastDamageCause = Player.DamageCause.ENEMY;
            player.OnHit(damage, Vector3.forward); // TODO: Add proper knockback direction 

            //TODO: implement knockback
            // player.applyKnockback(CalculatedKnockback(playerObject), attackKnockback);
        }

        private Vector3 CalculatedKnockback(PlayerObject playerObject)
        {
            Vector3 directionToPlayer = playerObject.transform.position - transform.position;
            directionToPlayer.y = 0;
            directionToPlayer.Normalize();

            return directionToPlayer * attackKnockback;
        }

        void Shot()
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            rb.linearVelocity = bulletDirection * bulletSpeed;

            // Destroy the bullet after its lifetime
            Destroy(gameObject, bulletLifeTime);
        }

        private void PlayImpactVFX()
        {
            if (impactVFX == null) return;

            // Instantiate VFX at the bullet's position and rotation
            GameObject vfxInstance = Instantiate(impactVFX, transform.position, Quaternion.identity);

            // Get the ParticleSystem component to check its duration
            ParticleSystem vfxParticleSystem = vfxInstance.GetComponent<ParticleSystem>();
            if (vfxParticleSystem != null)
            {
                Destroy(vfxInstance, vfxParticleSystem.main.duration + vfxParticleSystem.main.startLifetime.constantMax);
            }
            else
            {
                // If no ParticleSystem is found, destroy immediately
                Destroy(vfxInstance, 2f); // Default to 2 seconds
            }
        }

    }
}