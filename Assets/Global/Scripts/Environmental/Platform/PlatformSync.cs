using UnityEngine;

public class PlatformSync : MonoBehaviour
{
    private Vector3 prevPosition;

    private void OnCollisionEnter(Collision other)
    {
        prevPosition = this.transform.position;
    }

    private void OnCollisionStay(Collision other)
    {
        var currentPosition = transform.position;
        var diff = currentPosition - prevPosition;
        prevPosition = currentPosition;
        other.transform.position += diff;
    }
}
