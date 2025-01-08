using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    protected Vector3 offset = new Vector3(0, 0, 0);
    private CinemachineCamera cinemachineCamera;
    private SmoothTarget smoothCamaraTarget;

    public virtual void Start()
    {
        cinemachineCamera = GlobalReference.GetReference<PlayerReference>().CinemachineCamera;
        smoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget;
    }

    public virtual void Spawn()
    {
        Vector3 offset = new Vector3(0, 0, 3);

        var playerObj = GlobalReference.GetReference<PlayerReference>().PlayerObj;
        var rb = playerObj.GetComponent<Rigidbody>();
        rb.MovePosition(this.transform.position + offset);
        Quaternion targetRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z);
        rb.MoveRotation(targetRotation);

        var SmoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget;
        SmoothCamaraTarget.transform.position = this.transform.position + offset;
        SmoothCamaraTarget.transform.rotation = targetRotation;

        StartCoroutine(AdjustPositionAndRotation(1f));
    }

    private IEnumerator AdjustPositionAndRotation(float duration)
    {

        var SmoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget.transform;

        Vector3 newPosition = this.transform.position + new Vector3(0, 2, 8);
        Quaternion newRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z);

        cinemachineCamera.ForceCameraPosition(newPosition, newRotation);

        yield return new WaitForSeconds(duration);

        cinemachineCamera.Follow = SmoothCamaraTarget;
        cinemachineCamera.LookAt = SmoothCamaraTarget;

    }
}
