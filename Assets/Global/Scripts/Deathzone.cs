using System.Collections;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("The amount of damage , as % of maxhealth, to deal to the player when hitting this Deathzone")]
    private float damageAmount = 0.25f;
    [SerializeField]
    private PlayerSpawnPoint spawnPoint;
    void OnTriggerEnter(Collider other)
    {
        //check for player
        other.TryGetComponent<PlayerObject>(out PlayerObject player);

        if (player)
        {
            Debug.Log("PLAYER COLLISION DZ");
            Player playerRef = GlobalReference.GetReference<PlayerReference>().Player;
            Debug.Log($"Do {damageAmount * 100}% damage");
            playerRef.OnHit(playerRef.playerStatistic.MaxHealth.GetValue() * damageAmount, new Vector3(0, 0, 0));
            StartCoroutine(DelayRespawn());
            return;
        }

        //check for enemy
        other.TryGetComponent<EnemiesNS.EnemyBase>(out EnemiesNS.EnemyBase enemy);

        if (enemy)
        {
            // can enemies even fall? they dont have a RigidBody, due to navmeshagent.
            Debug.Log("ENEMY COLLISION DZ");
            // deal damage to equal the amount of MAX health times 2 just to be sure.
            enemy.OnHit(enemy.maxHealth * 2);
            return;
        }
    }

    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log($"Respawn in predefined spot: {spawnPoint.transform.position}");
        spawnPoint.SpawnPoint();
    }
}
