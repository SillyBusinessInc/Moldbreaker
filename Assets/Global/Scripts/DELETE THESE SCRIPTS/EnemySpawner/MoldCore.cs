using UnityEngine;

public class MoldCore : EnemiesNS.EnemyBase
{
    public Transform spawnArea { get; private set; }
    public float radius = 5;

    protected override void Start()
    {
        base.Start();
        GlobalReference.AttemptInvoke(Events.MOLD_CORE_SPAWNED);

        // Create the spawn area dynamically
        CreateSpawnArea();
    }

    private void CreateSpawnArea()
    {
        // Instantiate a new GameObject for the spawn area
        GameObject spawnAreaObject = new GameObject("SpawnArea");
        spawnArea = spawnAreaObject.transform;

        // Set the spawn area's position and parent it to the current object for organization
        spawnArea.position = transform.position;
        spawnArea.SetParent(transform);

        // Set the local scale based on the radius (X and Z axis)
        spawnArea.localScale = new Vector3(radius, 1, radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}