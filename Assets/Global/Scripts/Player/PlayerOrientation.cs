using UnityEngine;


public class PlayerOrientation : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform cameraTransform;

    private Vector2 _rotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (player.movementInput.magnitude > 0)
        {
            // Align the player with the camera's forward direction if forward movement is initiated
            AlignPlayerWithCamera();
            transform.position = player.rb.transform.position;
        }
    }

    private void AlignPlayerWithCamera()
    {
        // Get the forward direction of the camera and ignore the Y component
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Update the player's rotation to face the camera's horizontal direction
        if (cameraForward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = targetRotation;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward,
            Color.red, 0f, false);
    }
}
