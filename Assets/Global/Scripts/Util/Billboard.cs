using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;
    void Update()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
