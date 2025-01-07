using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

    private void OnTriggerEnter(Collider other)
    {
        GameObject entity = other.gameObject;
        if (tagHandlers.TryGetValue(entity.tag, out var handler))
        {
            handler(entity);
        }
    }

    private void HandlePlayer(GameObject entity)
    {
        if (player == null) return;

        if (CheckAndAddToHitEntities(entity))
        {
            player.Player.OnHit(damage, Vector3.up);
        }
    }

    private void HandleEnemy(GameObject entity)
    {

        var enemy = entity.GetComponent<EnemiesNS.EnemyBase>();
        if (enemy == null) return;

        if (CheckAndAddToHitEntities(entity))
        {
            enemy.OnHit(enemyDamage, knockbackForce, leapForce);
        }
    }

    private bool CheckAndAddToHitEntities(GameObject entity)
    {
        if (!hitEntities.Contains(entity))
        {
            hitEntities.Add(entity);
            StartCoroutine(ReEnableAfterDelay(entity));
            return true;
        }
        return false;
    }

    // Coroutine to re-enable the spike cone after a delay
    private IEnumerator ReEnableAfterDelay(GameObject entity)
    {
        yield return new WaitForSeconds(disableDuration);
        hitEntities.Remove(entity);
    }
}
