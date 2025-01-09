using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public Transform player; // The player or object the shadow follows
    //public LayerMask groundLayer; // LayerMask to identify the ground
    public float maxHeight = 10f; // Maximum height for shadow scaling
    public float minScale = 0.3f; // Minimum shadow scale
    public float maxScale = 1f; // Maximum shadow scale
    public float offset = 0.1f; // Offset above the ground

    private Transform shadowQuad;

    void Start()
    {
        // Find the Quad child object
        shadowQuad = transform.GetChild(0);
    }

    void Update()
    {
        RaycastToGround();
    }

    void RaycastToGround()
    {
        Ray ray = new Ray(player.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Position the shadow on the ground
            transform.position = hit.point + Vector3.up * offset;
            transform.rotation.SetLookRotation(hit.normal);

            // Adjust the scale based on height
            float height = Mathf.Clamp(hit.distance, 0, maxHeight);
            float scale = Mathf.Lerp(maxScale, minScale, height / maxHeight);
            shadowQuad.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            // Optionally disable the shadow if no ground is detected
           // shadowQuad.gameObject.SetActive(false);
        }
    }
}
