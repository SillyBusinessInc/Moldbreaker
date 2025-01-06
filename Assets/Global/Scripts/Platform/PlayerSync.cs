using UnityEngine;

public class PlayerSync : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Vector3 offset; // Distance between player and platform
    private Vector3 PlatformVelocity;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (IsOnPlatform())
        {
            Vector3 platformVelocity = PlatformVelocity;
            transform.position += platformVelocity * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Transform platformTransform = collision.transform;
            offset = transform.position - platformTransform.position;
            Rigidbody platformRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            PlatformVelocity = platformRigidbody ? platformRigidbody.linearVelocity : Vector3.zero;
        }
    }

    private bool IsOnPlatform()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Platform"));
    }
}
