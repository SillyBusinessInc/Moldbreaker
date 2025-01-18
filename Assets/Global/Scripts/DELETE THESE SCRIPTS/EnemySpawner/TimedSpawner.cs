using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using random = UnityEngine.Random;

public class TimedSpawner : MonoBehaviour
{
    public float spawnInterval = 1f; // Interval between spawns
    public float spawnDuration = 30f; // Total time to spawn enemies
    public List<EnemyPrefabCount> enemyChanceList; // List of enemies and their spawn chances
    public List<Transform> spawnAreas; // Area to spawn enemies

    private readonly SingleEnemySpawnArea spawner = new();
    private float startTime;
    private readonly List<GameObject> spawnedEnemies = new();
    [SerializeField] private int enemyLimit = 10;
    private bool isDead = false;

    void Start()
    {
        startTime = Time.time;
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        while (Time.time < spawnDuration + startTime)
        {
            spawnedEnemies.RemoveAll(item => item == null);

            if (spawnedEnemies.Count < enemyLimit)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        if (!isDead)
        {
            EndSpawning();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyprefab = RandomDistribution.GetRandom(EnemyPrefabCount.GetDict(enemyChanceList));
        Transform spawnArea = spawnAreas[random.Range(0, spawnAreas.Count)];
        if (enemyprefab != null && spawnArea != null)
        {
            GameObject spawnedEnemy = spawner.SpawnEnemy(enemyprefab, spawnArea, false);
            if (spawnedEnemy != null)
            {
                spawnedEnemies.Add(spawnedEnemy);
            }
        }
    }

    private void EndSpawning()
    {
        isDead = true;
        // Destroy all spawned enemies
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        // Destroy the spawner itself
        Destroy(gameObject);
        GlobalReference.AttemptInvoke(Events.NEXT_SPAWNER);
    }
}
