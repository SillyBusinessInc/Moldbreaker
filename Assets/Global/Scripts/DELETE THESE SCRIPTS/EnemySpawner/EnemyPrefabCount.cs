using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EnemyPrefabCount
{
    public GameObject enemyPrefab; // The enemy type
    public int amount;              // Number of this type to spawn

    public static Dictionary<GameObject, int> GetDict(List<EnemyPrefabCount> list)
    {
        return list.ToDictionary(x => x.enemyPrefab, x => x.amount);
    }
}