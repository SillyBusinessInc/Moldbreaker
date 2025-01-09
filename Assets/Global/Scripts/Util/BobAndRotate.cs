using UnityEngine;

public class PickupBobAndRotate : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 40f;  
    public Vector3 rotationAxis = Vector3.up;

    private Vector3 startPosition;  

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Bobbing motion
        float newY = startPosition.y + bobHeight * Mathf.Sin(Time.time * bobSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotation motion
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}