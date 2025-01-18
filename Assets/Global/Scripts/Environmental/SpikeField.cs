using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeField : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Damage dealt by the cone
    [SerializeField] private int enemyDamage = 10; // Damage dealt by the cone
    [SerializeField] private float cooldown = 1f; // Avoid damage loop
    [SerializeField] private float knockBackStrength = 3f; // Horizontal knockBack speed
    [Tooltip("The percentage of the knockback force that will be applied upwards, regardless of the direction that you hit the spikes")]
    [SerializeField, Range(0f,1f)] private float KnockBackUpPercentage = 0.75f;
 
    // It is static, so because of that, the cooldown is shared between all the spike cones
    private static readonly List<GameObject> hitEntities = new();
    private Dictionary<string, System.Action<GameObject, Collision>> tagHandlers;

    private void Start()
    {
        tagHandlers = new()
        {
            { "Player", HandlePlayer },
            { "Enemy", HandleEnemy }
        };
    }

    private void OnCollisionStay(Collision collision)
    {
        var entity = collision.gameObject;
        var isApplicableForDamage = tagHandlers.TryGetValue(entity.tag, out var handler);
        if (isApplicableForDamage) handler(entity, collision);
    }

    private void HandlePlayer(GameObject entity, Collision collision)
    {
        if (!this.AddToHitEntities(entity)) return;

        var player = GlobalReference.GetReference<PlayerReference>();
        player.Player.lastDamageCause = Player.DamageCause.HAZARD;
        var knockBackDirection = Vector3.up * this.KnockBackUpPercentage +
                                 collision.GetContact(0).normal * (1f - this.KnockBackUpPercentage);
        player.Player.OnHit(this.damage, knockBackDirection * knockBackStrength);
    }

    private void HandleEnemy(GameObject entity, Collision _)
    {
        var enemy = entity.GetComponent<EnemyBase>();
        if (enemy == null) return;

        if (AddToHitEntities(entity)) enemy.OnHit(enemyDamage);
    }

    private bool AddToHitEntities(GameObject entity)
    {
        if (hitEntities.Contains(entity)) return false;

        hitEntities.Add(entity);
        this.StartCoroutine(this.ReEnableAfterDelay(entity));
        return true;
    }

    // Coroutine to re-enable the spike cone after a delay
    private IEnumerator ReEnableAfterDelay(GameObject entity)
    {
        yield return new WaitForSeconds(cooldown);
        hitEntities.Remove(entity);
    }
}
