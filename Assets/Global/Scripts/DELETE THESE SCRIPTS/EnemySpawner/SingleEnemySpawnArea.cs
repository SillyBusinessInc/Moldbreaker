using UnityEngine;
using Random = UnityEngine.Random;

public class SingleEnemySpawnArea 
{
    public GameObject SpawnEnemy(GameObject enemyPrefab, Transform spawnArea, bool center)
    {
        Vector3 spawnPosition = GetRandomPointInSpawnArea(spawnArea, center);
        
        GameObject enemy = Object.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        return enemy;
    }

    private Vector3 GetRandomPointInSpawnArea(Transform spawnArea, bool center)
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
