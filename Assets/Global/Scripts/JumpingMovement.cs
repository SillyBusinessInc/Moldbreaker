using UnityEngine;

public class JumpingMovement : MonoBehaviour
{
    [SerializeField] private float bounceForceUp = 5.0f;
    [SerializeField] private float bounceForceForward = 2.0f;
    [SerializeField] private float topAngleThreshold = 105.0f;
    
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        
        if (rb) {
            foreach (ContactPoint contact in collision.contacts) {
                float angle = Vector3.Angle(contact.normal, Vector3.up);
                
                // if an object gets on top of the object this script is attached to
                if (angle >= topAngleThreshold) {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                    rb.AddForce(transform.up * bounceForceUp +  rb.transform.forward * bounceForceForward, ForceMode.Impulse); // bounce effect
                    break;
                }
            }
        }
    }
}
