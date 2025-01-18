using System.Collections;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("The amount of damage , as % of maxhealth, to deal to the player when hitting this Deathzone")]
    private float damageAmount = 0.25f;
    [SerializeField]
    private SpawnPoint spawnPoint;
    private CrossfadeController crossfadeController;
    private PlayerReference playerReference;

    void Start()
    {
        crossfadeController = GlobalReference.GetReference<CrossfadeController>();
        playerReference = GlobalReference.GetReference<PlayerReference>();
    }
    void OnTriggerEnter(Collider other)
    {
        //check for player
        other.TryGetComponent<PlayerObject>(out PlayerObject player);
        if (player)
        {
            StartCoroutine(PlayerRepsawnRoutine());
            return;
        }
        //check for enemy
        other.TryGetComponent<EnemiesNS.EnemyBase>(out EnemiesNS.EnemyBase enemy);

        if (enemy)
        {
            // deal damage to equal the amount of MAX health times 2 just to be sure.
            enemy.OnHit(enemy.maxHealth * 2);
            return;
        }
    }

    private IEnumerator PlayerRepsawnRoutine()
    {
        // get the actual player object to damage.
        Player playerRef = playerReference.Player;
        playerRef.lastDamageCause = Player.DamageCause.HAZARD;
        GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
        playerRef.OnHit(playerRef.playerStatistic.MaxHealth.GetValue() * damageAmount, new Vector3(0, 0, 0));

        // start the crossfade
        //if (crossfadeController) yield return StartCoroutine(crossfadeController.Crossfade_Start());

        // cancel out any movementinput and respawn the player on predefined spot
        playerRef.movementInput = new Vector2(0, 0);
        yield return StartCoroutine(DelayRespawn());
        // spawnPoint.Spawn();

        // clear the crossfade
        //if (crossfadeController) yield return StartCoroutine(crossfadeController.Crossfade_End());
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
    }

    private IEnumerator DelayRespawn()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        spawnPoint.Spawn();
    }
}
