using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeField : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Damage dealt by the cone
    [SerializeField] private int enemyDamage = 10; // Damage dealt by the cone
    [SerializeField] private float disableDuration = 0.1f; // Avoid damage loop
    [SerializeField] private float knockbackForce = 10f; // Horizontal knockback speed
    [SerializeField] private float leapForce = 5f; // Vertical leap speed
    private PlayerReference player;

    private List<GameObject> hitEntities = new();
    private Dictionary<string, System.Action<GameObject>> tagHandlers;

    private void Start()
    {
        // Ensure Player reference is set correctly
        player = GlobalReference.GetReference<PlayerReference>();
        if (player == null)
        {
            Debug.LogError("Player reference not found.");
        }

        // Initialize tag handlers
        tagHandlers = new()
        {
            { "Player", HandlePlayer },
            { "Enemy", HandleEnemy }
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject entity = collision.gameObject;
        if (tagHandlers.TryGetValue(entity.tag, out var handler))
        {
            handler(entity);
        }
    }

    private void HandlePlayer(GameObject entity)
    {
        if (player == null) return;

        if (AddToHitEntities(entity))
        {
            player.Player.OnHit(damage, Vector3.up);
            //ApplyKnockback(player.Player.gameObject);
        }
    }

    private void HandleEnemy(GameObject entity)
    {
        EnemyBase enemy = entity.GetComponent<EnemyBase>();
        if (enemy == null) return;

        if (AddToHitEntities(entity))
        {
            enemy.OnHit(enemyDamage);
            ApplyKnockback(enemy.gameObject);
        }
    }

    private bool AddToHitEntities(GameObject entity)
    {
        if (!hitEntities.Contains(entity))
        {
            hitEntities.Add(entity);
            StartCoroutine(ReEnableAfterDelay(entity));
            return true;
        }
        return false;
    }

    private void ApplyKnockback(GameObject entity)
    {
        Vector3 knockbackVelocity = CalculateKnockback(entity);
        player.Player.ApplyKnockback(knockbackVelocity, 3);
    }

    public virtual Vector3 CalculateKnockback(GameObject entityObject)
    {
        Vector3 directionToEntity = entityObject.transform.position - transform.position;
        directionToEntity.Normalize();

        Vector3 knockbackVelocity = directionToEntity * knockbackForce;
        knockbackVelocity.y = leapForce; // Add upward velocity for a leap

        return knockbackVelocity * 3;
    }

    // Coroutine to re-enable the spike cone after a delay
    private IEnumerator ReEnableAfterDelay(GameObject entity)
    {
        yield return new WaitForSeconds(disableDuration);
        hitEntities.Remove(entity);
    }
}
