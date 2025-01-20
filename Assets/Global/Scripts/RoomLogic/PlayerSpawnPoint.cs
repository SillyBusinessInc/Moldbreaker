public class PlayerSpawnPoint : SpawnPoint
{
    public override void Start()
    {
        base.Start();
        Spawn();
    }
}