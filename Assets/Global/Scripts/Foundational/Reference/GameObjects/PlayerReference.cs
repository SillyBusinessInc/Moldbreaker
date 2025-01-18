using Unity.Cinemachine;
using UnityEngine;

public class PlayerReference : Reference
{
    private Player player;
    public Player Player {
        get => player ? player : player = GetComponent<Player>();
    }

    private PlayerObject playerObj;
    public PlayerObject PlayerObj {
        get => playerObj ? playerObj : playerObj = GetComponentInChildren<PlayerObject>();
    }


    private SmoothTarget smoothCamaraTarget;
    public SmoothTarget SmoothCamaraTarget {
        get => smoothCamaraTarget ? smoothCamaraTarget : smoothCamaraTarget = GetComponentInChildren<SmoothTarget>();
    }

    private Camera playerCamera;
    public Camera PlayerCamera
    {
       get => playerCamera ? playerCamera : playerCamera = GetComponentInChildren<Camera>();
    }

    private CinemachineCamera cinemachineCamera;
    public CinemachineCamera CinemachineCamera
    {
       get => cinemachineCamera ? cinemachineCamera : cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
    }

    public GameObject playerBradleyWaffletail;
}
