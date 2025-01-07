using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace EnemiesNS
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Base Enemy Fields")]
        [Tooltip("HP for this enemy: integer")]
        [SerializeField]
        [Range(0, 1000)]
        public int health = 100;
        [HideInInspector]
        public int maxHealth;
        [Header("Base idle settings | ignored on moldcores ")]
        [Tooltip("For how long the enemy will idle before roaming to new position. NOTE: this is the base value, there will be randomisation applied to make it the idling seem more natural")]
        [SerializeField]
        [Range(0f, 300f)]
        public float idleTime = 0.5f;

        [Tooltip("Idle variance allows to finetune the randomisation of the idle time, 0.5 means idle variance will be between 50% and 150% of the given idle time.")]
        [SerializeField]
        [Range(0f, 1f)]
        public float idleVariance = 0.5f;

        [HideInInspector] public float idleWaitElapsed;
        [HideInInspector] public float idleWaitTime;
        [HideInInspector] public bool isIdling = false;

        [Header("Base roam settings | ignored on moldcores")]
        [Tooltip("How far this enemy will travel from it's original spawn while roaming | ignored on moldcores")]
        [SerializeField]
        [Range(0f, 250f)]
        public float roamRange = 15f;

        [Tooltip("Movement speed while roaming")]
        [SerializeField]
        [Range(0f, 250f)]
        public float roamingSpeed = 3.5f;

        [Tooltip("Acceleration while roaming")]
        [SerializeField]
        [Range(0f, 250f)]
        public float roamingAcceleration = 8f;

        [HideInInspector] public Vector3 spawnPos;
        [HideInInspector] public Vector3 roamDestination;

        [Header("Base chase settings | ignored on moldcores")]
        [Tooltip("How close the target needs to get before triggering the chasing behavior")]
        [SerializeField]
        [Range(0f, 250f)]
        public float chaseRange = 25f;

        [Tooltip("Movement speed while chasing")]
        [SerializeField]
        [Range(0f, 250f)]
        public float chaseSpeed = 3.5f;

        [Tooltip("Acceleration while chasing")]
        [SerializeField]
        [Range(0f, 250f)]
        public float chaseAcceleration = 8f;

        [Tooltip("the minimum distance to keep from the player.")]
        [SerializeField]
        [Range(0f, 5f)]
        public float minDistanceToPlayer = 2f;

        [Tooltip("Time for the chasing enemy to hold position once it gets into attackingrange but still on attack cooldown. Used to keep the enemy from hugging the player.")]
        [SerializeField]
        [Range(0f, 5f)]
        public float chaseWaitTime = 1f;

        [HideInInspector] public float chaseWaitElapsed = 0;
        [HideInInspector] public bool isChasing = false;
        [HideInInspector] public bool isWaiting = false;
        [HideInInspector] public float distanceToPlayer;

        [Header("Base attack settings | ignored on moldcores")]
        [Tooltip("The range of the attack")]
        [SerializeField]
        [Range(0f, 250f)]
        public float attackRange = 2f;

        [Tooltip("The time between attacking states")]
        [SerializeField]
        [Range(0f, 300f)]
        public float attackCooldown = 2f;

        [Tooltip("The amount of time this character will have to recover from attacking, and be standing still before able to attack again. NOTE: if this is less than the attack clip length, there will be no additional waiting time applied.")]
        [SerializeField]
        [Range(0f, 10f)]
        public float attackRecoveryTime = 0.3f;

        [Tooltip("number of attacks before triggering cooldown.")]
        [SerializeField]
        [Range(1, 10)]
        public int attacksPerCooldown = 1;

        [Tooltip("The base damage of the attack")]
        [SerializeField]
        [Range(0, 10)]
        public int attackDamage = 1;

        [Tooltip("The angle the enemy can be off while trying to face the player")]
        [SerializeField]
        [Range(0f, 180f)]
        public float facingPlayerVarianceAngle = 5f;

        [Tooltip("The amount of knockback this enemy's attacks will apply")]
        [SerializeField]
        [Range(0f, 100f)]
        public float attackKnockback;

        [Tooltip("The time the hit object will be stunned due to knockback")]
        [SerializeField]
        [Range(0f, 5f)]
        public float knockbackStunTime = 0.5f;
        [HideInInspector]
        public bool canAttack = true;
        [HideInInspector]
        public bool isRecovering = false;
        [HideInInspector]
        public float attackCooldownElapsed = 0;
        [HideInInspector]
        public float attackRecoveryElapsed = 0;
        [HideInInspector]
        public bool inAttackAnim = false;
        [HideInInspector]
        public bool HealthBarDestroy = false;

        [Header("Death Particle Effect settings")]
        [Tooltip("Reference to the Enemies Particle Death Prefab")]
        [SerializeField]
        private ParticleSystem PrefabDeathParticles;
        [Tooltip("Origin point for the death particle effect")]
        [SerializeField]
        private Transform DeathParticleOrigin;

        private ParticleSystem particleSystemDeath;

        [Header("References")]
        [Tooltip("OPTIONAL: Reference to the target's Transform. Default: Player")]
        [SerializeField]
        public Transform target;

        [Tooltip("OPTIONAL: Reference to the Animator of this enemy. Has Default")]
        [SerializeField]
        public Animator animator;

        [Tooltip("OPTIONAL: Reference to the NavMeshAgent of this enemy. Has Default")]
        [SerializeField]
        public NavMeshAgent agent;

        [Tooltip("OPTIONAL: Reference to the Rigidbody of this enemy. Has Default")]
        [SerializeField]
        public Rigidbody rb;

        [Tooltip("Reference to this enemy's weapon")]
        [SerializeField] public Collider weapon;

        [Tooltip("Reference to this Enemy's walking particle system")]
        public ParticleSystem particleSystemWalk;

        [Tooltip("Miscellaneous")]
        [HideInInspector]
        public int VFXLayer;


        [Header("States")]
        [HideInInspector] public BaseStates states;
        [HideInInspector] public StateBase currentState;

        [Header("DEBUGGING")]
        [Tooltip("DO NOT SET | shows the current state's name")]
        [SerializeField]
        protected string currentStateName;

        [Tooltip("DO NOT SET | shows the current state's name")]
        [SerializeField]
        protected bool agentIsStopped = false;

        [Header("Visual")]
        [SerializeField] private SkinnedMeshRenderer moldRenderer;
        private float targetMoldPercentage = 1;
        private float currentMoldPercentage = 1;

        //TODO: this is a quick fix to get the demo out the door, make this nicer
        // this should be cleaned up and placed higher up somewhere

        protected virtual void Start()
        {
            maxHealth = health;
            spawnPos = this.transform.position;
            setReferences();
            SetupStateMachine();
            GlobalReference.AttemptInvoke(Events.ENEMY_SPAWNED);
        }

        protected virtual void Update()
        {
            agentIsStopped = agent.isStopped;
            UpdateTimers();
            currentState?.Update();

            currentMoldPercentage -= (currentMoldPercentage - targetMoldPercentage) * 2 * Time.deltaTime;
            moldRenderer.material.SetFloat("_MoldStrength", currentMoldPercentage > 0 ? currentMoldPercentage * 0.6f + 0.2f : currentMoldPercentage);
        }

        protected void FxedUpdate() => currentState?.FixedUpdate();

        public virtual void OnHit(int damage, float force = 0f, float leapForce = 0f)
        {
            health -= damage;
            if (animator) animator.SetTrigger("PlayDamageFlash");

            if (health <= 0)
            {
                OnDeath();
                return;
            }

            if (!animator) return;
            if (!inAttackAnim) animator.SetTrigger("PlayDamage");

            if (force > 0)
            {
                // backwards knockback
                Vector3 kb = -transform.forward * force;

                // apply knockback
                kb.y += leapForce;

                DoKnockback(kb, knockbackStunTime);
            }

            float p = health/(float)maxHealth;
            Debug.LogWarning($"[{p}] health: {health}, maxHealth: {maxHealth}");
            targetMoldPercentage = p;
        }

        protected virtual void OnDeath()
        {
            HealthBarDestroy = true;
            ChangeState(states.Dead);
        }

        protected virtual void OnDestroy()
        {
            GlobalReference.AttemptInvoke(Events.ENEMY_KILLED);
        }



        public void   DoKnockback(Vector3 knockback, float duration)
        {

            FreezeMovement(true);
            rb.isKinematic = false;
            rb.useGravity = true;

            // apply knockback
            rb.AddForce(knockback, ForceMode.Impulse);

            StartCoroutine(AfterKnockback(duration));
        }

        private IEnumerator AfterKnockback(float duration)
        {

            float knockbackTime = Time.deltaTime;
            // if the enemy is knocked back for more than X (duration) seconds or the velocity is less than 0.1f, stop the knockback
            yield return new WaitUntil(
                () => Time.deltaTime - knockbackTime > duration || rb.linearVelocity.magnitude < 0.1f
            );
            // just to be sure, wait a little bit more
            yield return new WaitForSeconds(0.25f);

            rb.isKinematic = true;
            rb.useGravity = false;

            FreezeMovement(false);

            yield return null;

        }

        public void ChangeState(StateBase state)
        {
            currentState?.Exit();
            currentState = state;
            currentState?.Enter();
            currentStateName = state.GetType().Name;
        }

        protected void setReferences()
        {
            if (!target)
            {
                try
                {
                    target = GlobalReference.GetReference<PlayerReference>().PlayerObj.transform;
                }
                catch { }
            }
            if (!agent)
            {
                agent = this.GetComponent<NavMeshAgent>();
            }
            // ANIMATOR IS CAUSE PROBLEMS WHEN ENEMIES DONT HAVE ANY. But MOLD CORES DONT HAVE ANIMATORS
            // Note that its also a bit weird if we have a SetReference method, and have the fields be serialized.
            // Either make it through getComponent if its alsoways on the same object.
            // Or make it serializable if you allow it to be on a different object
            /*
            if (!animator)
            {
                animator = this.GetComponent<Animator>();
                VFXLayer = animator.GetLayerIndex("VFX");
            } 
            */
            if (!rb) rb = this.GetComponent<Rigidbody>();

            if (!DeathParticleOrigin)
            {
                Debug.LogWarning("NULLREFERENCE: Death Paricle Origin not set. This will result in malfunctioning OnDeath() behavior.", this);
            }
        }

        public void toggleCanAttack(bool v)
        {
            canAttack = v;
            if (canAttack) attackCooldownElapsed = 0f;
        }

        public void toggleIsRecovering(bool v)
        {
            isRecovering = v;
            if (!isRecovering) attackRecoveryElapsed = 0f;
        }

        public void toggleIsIdling(bool v)
        {
            isIdling = v;
            if (!isIdling) idleWaitElapsed = 0f;
        }

        public void toggleIsWaiting(bool v)
        {
            isWaiting = v;
            if (!isWaiting) chaseWaitElapsed = 0f;
        }

        public void FreezeMovement(bool v)
        {
            agent.isStopped = v;
        }

        public void UpdateTimers()
        {
            // increment timers
            float elapsedTime = Time.deltaTime;
            if (isIdling) idleWaitElapsed += elapsedTime;
            if (isWaiting) chaseWaitElapsed += elapsedTime;
            if (!canAttack) attackCooldownElapsed += elapsedTime;
            if (isRecovering) attackRecoveryElapsed += elapsedTime;

            // check flags
            if (idleWaitElapsed >= idleWaitTime) toggleIsIdling(false);
            if (chaseWaitElapsed >= chaseWaitTime) toggleIsWaiting(false);
            if (attackCooldownElapsed >= attackCooldown) toggleCanAttack(true);
            if (attackRecoveryElapsed >= attackRecoveryTime) toggleIsRecovering(false);
        }

        public virtual void PlayerHit(PlayerObject playerObject, int damage, Vector3 direction)
        {
            Player player = playerObject.GetComponentInParent<Player>();
            if (!player) return;
            player.OnHit(damage, transform.forward);
        }

        public virtual void EnableWeaponHitBox() { }
        public virtual void DisableWeaponHitBox() { }

        public virtual Vector3 CalculatedKnockback(PlayerObject playerObject)
        {
            Vector3 directionToPlayer = playerObject.transform.position - transform.position;
            directionToPlayer.y = 0;
            directionToPlayer.Normalize();

            return directionToPlayer * attackKnockback;
        }


        //
        // Used as events in animations
        //
        public void AttackAnimStarted()
        {
            inAttackAnim = true;
        }
        public void AttackAnimEnded()
        {
            toggleIsRecovering(true); // this seems more fitting to start recovery time. after the attack has finished. rather than on attack start like before.
            inAttackAnim = false;
        }
        public void DeathAnimEnded()
        {
            targetMoldPercentage = 0;
            // if (animator) animator.SetBool("Idle", true);
            currentMoldPercentage = 0;

            GetComponentInChildren<Collider>().enabled = false;

            GlobalReference.AttemptInvoke(Events.ENEMY_KILLED);
            // animator is on the Model's GameObject, so we can reach that GameObject through this.
            if (animator)
            {
                StartCoroutine(DisableAfter(animator.gameObject, 0.5f));
            }

            // Instantiate and play the death particle effect
            if (!DeathParticleOrigin) return;
            particleSystemDeath = Instantiate(PrefabDeathParticles, DeathParticleOrigin);
            particleSystemDeath.Play();

            // Start a coroutine to destroy the particle system and the enemy once the particles finish playing
            StartCoroutine(DestroyAfterParticles(particleSystemDeath));
        }

        public IEnumerator DisableAfter(GameObject obj, float time) {
            yield return new WaitForSeconds(time);
            obj.SetActive(false);
        }

        //
        // When creating your own enemy, override this to use your enemy specific BaseStates class. And set the set to your desired default state.
        //
        protected virtual void SetupStateMachine()
        {
            states = new BaseStates(this);
            ChangeState(states.Idle);
        }

        void OnDrawGizmos()
        {
            // Draw the Chase Range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

            // Draw the Minimum Distance from Player
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, minDistanceToPlayer);

            // Draw the Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Draw the Roam Range around the spawn position
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spawnPos, roamRange);
        }



        private IEnumerator DestroyAfterParticles(ParticleSystem particles)
        {
            if (particles != null)
            {
                // Wait until the particle system finishes playing
                yield return new WaitWhile(() => particles.isPlaying);

                // Destroy the particle system
                Destroy(particles.gameObject);
            }

            // Destroy the enemy object
            Destroy(this.gameObject);
        }
    }
}
