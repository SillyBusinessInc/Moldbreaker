using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private SmoothTarget smoothCamaraTarget;
    private CameraManager cameraManager;
    private Vector3 previousPosition;
    private Vector3 presentPosition;
    private Vector3 delta;

    public void Start() {
        cinemachineCamera = GlobalReference.GetReference<PlayerReference>().CinemachineCamera;
        smoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget;
        cameraManager = GlobalReference.GetReference<PlayerReference>().CameraManager;
        previousPosition = smoothCamaraTarget.transform.position;

        SpawnPoint();

        presentPosition = smoothCamaraTarget.transform.position;
        delta = presentPosition - previousPosition;

        // move the camera immidiately to the smoothCamaraTarget's transform
        // cinemachineCamera.OnTargetObjectWarped(smoothCamaraTarget.transform, delta);
    }

    public void SpawnPoint() {
        Vector3 offset = new Vector3(0, 0, 3);

        var playerObj = GlobalReference.GetReference<PlayerReference>().PlayerObj;
        var rb = playerObj.GetComponent<Rigidbody>();
        rb.MovePosition(this.transform.position + offset);
        Quaternion targetRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z);
        rb.MoveRotation(targetRotation);

        var SmoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget;
        SmoothCamaraTarget.transform.position = this.transform.position + offset;
        SmoothCamaraTarget.transform.rotation = targetRotation;

        // var cinemachineCamera = GlobalReference.GetReference<PlayerReference>().CinemachineCamera;
        // cinemachineCamera.transform.position = this.transform.position + new Vector3(0, 2, 3); 
        // cinemachineCamera.transform.rotation = targetRotation;

        // var playerCamera = GlobalReference.GetReference<PlayerReference>().PlayerCamera;
        // playerCamera.transform.position = this.transform.position + new Vector3(0, 2, 3); 
        // playerCamera.transform.rotation = targetRotation;
        
        // cameraManager.SetManualPositionAndRotation(this.transform.position + offset, targetRotation);
        // cameraManager.CameraRotation();
        // RotationDriveMode composer = cinemachineCamera.GetComponent<RotationDriveMode>();
        StartCoroutine(AdjustPositionAndRotation(1f));
    }

    private IEnumerator AdjustPositionAndRotation(float duration) { 
        
        var SmoothCamaraTarget = GlobalReference.GetReference<PlayerReference>().SmoothCamaraTarget.transform;
        
        // Quaternion targetRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z);
        
        // cinemachineCamera.Follow = null; 
        // cinemachineCamera.LookAt = null; 
        // cinemachineCamera.transform.position = this.transform.position + new Vector3(0, 2, 8); ; 
        // cinemachineCamera.transform.rotation = targetRotation; 

        // yield return new WaitForSeconds(duration); 
        
        // cinemachineCamera.Follow = SmoothCamaraTarget; 
        // cinemachineCamera.LookAt = SmoothCamaraTarget;


        // 새로운 위치와 회전값 설정 
        Vector3 newPosition = this.transform.position + new Vector3(0, 2, 8); 
        Quaternion newRotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 180, this.transform.rotation.eulerAngles.z); 
        
        // 카메라 위치와 회전을 강제로 설정
        cinemachineCamera.ForceCameraPosition(newPosition, newRotation);

        yield return new WaitForSeconds(duration); 
        
        cinemachineCamera.Follow = SmoothCamaraTarget; 
        cinemachineCamera.LookAt = SmoothCamaraTarget;

    }
}
