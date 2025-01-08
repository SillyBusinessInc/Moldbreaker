using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawnPoint : SpawnPoint
{
    private CinemachineCamera cinemachineCamera;
    private SmoothTarget smoothCamaraTarget;

    public override void Start()
    {
        base.Start();
        offset = new Vector3(0, 0, 3);
        Spawn();
    }
}