using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class EnemyWaveManager : MonoBehaviour
{
    public List<Wave> waves;
    public List<Transform> spawnAreaTransforms;

    private List<GameObject> activeEnemies = new List<GameObject>();
    // private int totalEnemies;
    // private int deadEnemies;
    private int currentWave = 0;
    private int wavesDone = 0;
    private int maxWaves;
    private bool nextWave = false;
    
    private void Update()
    {
        if (activeEnemies.Count() == 0 || activeEnemies.All(enemy => enemy == null) && nextWave)
        {
            WaveCompleted();
        }
    }

    private void StartWaveCoroutine() => StartCoroutine(StartWave());
    
    private void OnEnable()
    {
        // deadEnemies = 0;
        maxWaves = waves.Count;
        StartWaveCoroutine();
        GlobalReference.SubscribeTo(Events.NORMAL_WAVE_START, StartWaveCoroutine);
        GlobalReference.SubscribeTo(Events.NORMAL_WAVE_DONE, WaveCompleted);
        GlobalReference.SubscribeTo(Events.ENEMY_KILLED, OnEnemyDeath);
    }

    private void OnDestroy()
    {
        GlobalReference.UnsubscribeTo(Events.NORMAL_WAVE_START, StartWaveCoroutine);
        GlobalReference.UnsubscribeTo(Events.NORMAL_WAVE_DONE, WaveCompleted);
        GlobalReference.UnsubscribeTo(Events.ENEMY_KILLED, OnEnemyDeath);
    }
    public IEnumerator StartWave()
    {
        nextWave = false;
        
        // deadEnemies = 0;
        // totalEnemies = 0;
        
        var spawner = gameObject.GetComponent<WaveSpawnArea>();
        var totalEnemies = waves[currentWave].waveParts.Sum(wavePart => wavePart.enemyPrefabs.Sum(enemy => enemy.amount));
        nextWave = true;

        foreach (var wavePart in waves[currentWave].waveParts) // Access waveParts within each Wave
        {
            spawner.maxSpawnedEnemies = totalEnemies; // Use 'count' if that's the property name
            if (wavePart.spawnArea >= 0)
            {
                spawner.spawnArea = spawnAreaTransforms[wavePart.spawnArea];
                spawner.center = wavePart.center;
                foreach (var enemy in wavePart.enemyPrefabs) // Loop through each enemy in wavePart
                {
                    // Access the spawn area component and set the enemy properties
                    spawner.enemyPrefab = enemy.enemyPrefab;
                    spawner.waveDone = true;
                    foreach (var _ in Enumerable.Range(0, enemy.amount))
                    {
                        var enemyObject =spawner.SpawnEnemy();
                        activeEnemies.Add(enemyObject);
                        yield return new WaitForSeconds(waves[currentWave].interval);
                    }
                }
            }
            else
            {
                Debug.LogError("No spawn area found");
            }
        }
    }

    private void OnEnemyDeath()
    {
        if (!activeEnemies.Any() || activeEnemies.All(enemy => enemy == null))
        {
            WaveCompleted();
        }
    }

    private void WaveCompleted()
    {   
        nextWave = false;
        // totalEnemies = 0;
        wavesDone++;
        if (wavesDone >= maxWaves)
        {
            GlobalReference.AttemptInvoke(Events.ALL_WAVES_DONE);
            GlobalReference.AttemptInvoke(Events.NEXT_SPAWNER);
            Destroy(gameObject);
        }
        else
        {
            nextWave = true;
            currentWave++;
            GlobalReference.AttemptInvoke(Events.NORMAL_WAVE_START);
        }
    }
}