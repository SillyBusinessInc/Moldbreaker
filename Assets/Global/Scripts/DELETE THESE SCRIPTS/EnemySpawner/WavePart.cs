using System.Collections.Generic;

[System.Serializable]
public class WavePart
{
    public List<EnemyPrefabCount> enemyPrefabs; // List to replace dictionary
    public int spawnArea; // Possible spawn areas for this wave
    public bool center = false; // Center the spawn area
}