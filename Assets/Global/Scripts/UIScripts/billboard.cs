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
        transform.rotation = mainCamera.transform.rotation;
    }
}
