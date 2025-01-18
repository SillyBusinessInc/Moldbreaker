using UnityEngine;
using System.Collections.Generic;
using random = UnityEngine.Random;

public class MoldCoreSpawner : MonoBehaviour
{
    public float interfal = 1;
    public List<EnemyPrefabCount> enemyChanceList;
    // public Transform spawnArea;
    [SerializeField] private List<MoldCore> cores = new();
    private SingleEnemySpawnArea spawner = new SingleEnemySpawnArea();
    private float currentTime;
    [SerializeField] private int limit = 10;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    List<GameObject> toRemove = new();
    private bool isDead = false;

    private void OnDeath()
    {
        isDead = true;
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
                GlobalReference.AttemptInvoke(Events.ENEMY_KILLED);
            }
        }
        Destroy(gameObject);
        GlobalReference.AttemptInvoke(Events.NEXT_SPAWNER);
    }

    void Update()
    {
        if (Time.time >= interfal+currentTime && spawnedEnemies.Count < limit)
        {            
            currentTime = Time.time;
            SpawnEnemy();
        }

        if (cores.TrueForAll(core => core == null) && !isDead)
        {
            OnDeath();
        }

        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy == null)
            {
                toRemove.Add(enemy);
            }
        }

        foreach (GameObject enemy in toRemove)
        {
            spawnedEnemies.Remove(enemy);
            currentTime = Time.time;
        }

        toRemove.Clear();
    }

    //spawn enemy function here with singleenemyspawner class
    public void SpawnEnemy()
    {   
        //try to get a new random core if the core is null with linq
        MoldCore singleCore = cores[random.Range(0, cores.Count)];
        while (singleCore == null)
        {
            singleCore = cores[random.Range(0, cores.Count)];
        }
        
        GameObject enemy = spawner.SpawnEnemy(RandomDistribution.GetRandom(EnemyPrefabCount.GetDict(enemyChanceList)), singleCore.spawnArea, false);
        if (enemy != null)
        {
            spawnedEnemies.Add(enemy);
            GlobalReference.AttemptInvoke(Events.ENEMY_SPAWNED);
        }
    }   
}