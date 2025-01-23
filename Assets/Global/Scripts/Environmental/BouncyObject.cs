using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private float bounceForceUp = 5.0f;
    [SerializeField] private float bounceForceForward = 2.0f;
    [SerializeField] private float topAngleThreshold = 105.0f;

    [SerializeField] private Animator animator;

    void OnCollisionEnter(Collision collision)
    {
        var rb = collision.rigidbody;
        if (!rb) return;

        foreach (var contact in collision.contacts)
        {
            var angle = Vector3.Angle(contact.normal, Vector3.up);

            // if an object gets on top of the object this script is attached to
            if (!(angle >= topAngleThreshold))
                continue;
            if(animator) animator.SetTrigger("Bounce");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * bounceForceUp + rb.transform.forward * bounceForceForward, ForceMode.Impulse); // bounce effect
            break;
        }
    }
}
