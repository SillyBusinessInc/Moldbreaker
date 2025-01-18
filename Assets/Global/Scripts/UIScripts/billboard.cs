using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GlobalReference.GetReference<PlayerReference>().PlayerCamera;
    }
    void Update()
    {
        // transform.LookAt(mainCamera.transform);
        // transform.Rotate(0, 180, 0);
        transform.rotation = mainCamera.transform.rotation;
    }
}
