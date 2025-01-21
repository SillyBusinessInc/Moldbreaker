using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] protected Vector3 offset = new Vector3(0, 0, 0);
    private CinemachineCamera cinemachineCamera;
    private SmoothTarget smoothCamaraTarget;

    [Header("Camera Settings")]
    [SerializeField] private float cameraDistance = 8f;
    [SerializeField] private float initializationDelay = 0.1f;

    public virtual void Start()
    {
        var playerRef = GlobalReference.GetReference<PlayerReference>();
        cinemachineCamera = playerRef.CinemachineCamera;
        smoothCamaraTarget = playerRef.SmoothCamaraTarget;

        if (IsInitialSpawnPoint())
        {
            StartCoroutine(InitialSpawn());
        }
    }

    private bool IsInitialSpawnPoint()
    {
        return gameObject.CompareTag("PlayerSpawnPoint");
    }

    private IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(initializationDelay);

        if (smoothCamaraTarget != null)
        {
            smoothCamaraTarget.transform.position = transform.position + offset;
            smoothCamaraTarget.transform.rotation = transform.rotation;
        }

        Spawn();
    }

    public virtual void Spawn()
    {
        var playerRef = GlobalReference.GetReference<PlayerReference>();
        var playerObj = playerRef.PlayerObj;
        var rb = playerObj.GetComponent<Rigidbody>();

        Quaternion targetRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + 180,
            transform.rotation.eulerAngles.z);

        // Position and rotate player
        rb.MovePosition(transform.position + offset);
        rb.MoveRotation(targetRotation);

        // Update smooth target to match player
        if (smoothCamaraTarget != null)
        {
            smoothCamaraTarget.transform.position = transform.position + offset;
            smoothCamaraTarget.transform.rotation = targetRotation;
        }

        StartCoroutine(AdjustCamera());
    }

    private IEnumerator AdjustCamera()
    {
        if (cinemachineCamera == null)
        {
            Debug.LogError("[SpawnPoint] CinemachineCamera is null!");
            yield break;
        }

        // Calculate camera position behind the player (same direction as player facing)
        Vector3 cameraOffset = transform.forward * cameraDistance;
        Vector3 newCameraPosition = transform.position + offset + cameraOffset;

        Quaternion newCameraRotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + 180,
            transform.rotation.eulerAngles.z);

        cinemachineCamera.ForceCameraPosition(newCameraPosition, newCameraRotation);

        yield return null;

        cinemachineCamera.Follow = smoothCamaraTarget.transform;
        cinemachineCamera.LookAt = smoothCamaraTarget.transform;
    }

    private void OnDrawGizmos()
    {
        // Draw spawn position
        Gizmos.color = Color.green;
        Vector3 spawnPos = transform.position + offset;
        Gizmos.DrawWireSphere(spawnPos, 0.5f);

        // Draw expected camera position
        Gizmos.color = Color.yellow;
        Vector3 cameraPos = spawnPos + (transform.forward * cameraDistance); // Removed the negative
        Gizmos.DrawWireSphere(cameraPos, 0.3f);

        // Draw line from spawn to camera
        Gizmos.color = Color.red;
        Gizmos.DrawLine(spawnPos, cameraPos);

        // Draw player forward direction
        Gizmos.color = Color.blue;
        Quaternion playerRot = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + 180,
            transform.rotation.eulerAngles.z);
        Vector3 playerForward = playerRot * Vector3.forward;
        Gizmos.DrawRay(spawnPos, playerForward * 2f);
    }
}