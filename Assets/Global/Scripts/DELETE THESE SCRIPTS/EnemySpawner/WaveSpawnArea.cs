using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class WaveSpawnArea : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Prefab of the enemy to spawn")]
    [HideInInspector] public GameObject enemyPrefab;

    [HideInInspector] public int maxSpawnedEnemies;

    [Header("Spawn Area Settings")]
    [HideInInspector] public Transform spawnArea;

    private List<GameObject> activeEnemies = new List<GameObject>();

    [HideInInspector] public bool center = false;

    [HideInInspector] public bool waveDone = false;

    private void Update()
    {
        if(activeEnemies.Count == maxSpawnedEnemies && activeEnemies.TrueForAll(enemy => enemy == null) &&waveDone)
        {
            waveDone = false;
            GlobalReference.AttemptInvoke(Events.NORMAL_WAVE_DONE);
        }
    }

    public GameObject SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomPointInSpawnArea();
        
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
        GlobalReference.AttemptInvoke(Events.ENEMY_SPAWNED);
        return newEnemy;
    }

    private Vector3 GetRandomPointInSpawnArea()
    {
        Vector3 areaCenter = spawnArea.position;
        Vector3 areaSize = spawnArea.localScale;

        if (center)
        {
            return areaCenter;
        }
        // Randomize a point within the spawn area's bounds
        float xPos = Random.Range(-(areaSize.x / 2), (areaSize.x / 2));
        float zPos = Random.Range(-(areaSize.z / 2), (areaSize.z / 2));
        
        // You can adjust this if you want to spawn on a specific surface (e.g., ground level)
        return new Vector3(areaCenter.x + xPos, areaCenter.y, areaCenter.z + zPos);
    }
}
