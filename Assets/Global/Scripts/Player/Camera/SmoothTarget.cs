using UnityEngine;

public class SmoothTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField]
    [Range(0f, 10f)]
    private float smoothSpeed = 5f;

    public void LateUpdate()
    {
        // Smoothly interpolate position to match target's position
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * smoothSpeed);
        transform.position = targetPosition;
    }

    private void OnDrawGizmos()
    {
        if (target == null)
            return;

        // Draw the player position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target.position, 0.1f);

        // Draw the smoothed target position
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.transform.position, 0.1f);

        // Draw a line between the player and the smoothed target
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(target.position, this.transform.position);
    }
}
