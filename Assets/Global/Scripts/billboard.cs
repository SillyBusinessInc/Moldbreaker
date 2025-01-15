using UnityEngine;
using UnityEngine.Rendering;

public class billboard : MonoBehaviour
{
    private Transform cam;
    void Start()
    {
        cam = GlobalReference.GetReference<PlayerReference>().PlayerCamera.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }   
}
