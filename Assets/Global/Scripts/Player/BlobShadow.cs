using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public Transform player; // the player or object the shadow follows
    public float maxHeight = 10f; // maximum height for shadow scaling
    public float minScale = 0.3f; // minimum shadow scale
    public float maxScale = 1f; // maximum shadow scale
    public float offset = 0.1f; // offset above the ground

    private Transform shadowQuad;

    void Start()
    {
        // find the Quad child object
        shadowQuad = transform.GetChild(0);
    }

    void Update()
    {
        RaycastToGround();
    }

    void RaycastToGround()
    {
        Ray ray = new(player.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // position the shadow on the ground
            transform.position = hit.point + Vector3.up * offset;
            transform.rotation.SetLookRotation(hit.normal);

            // adjust the scale based on height
            float height = Mathf.Clamp(hit.distance, 0, maxHeight);
            float scale = Mathf.Lerp(maxScale, minScale, height / maxHeight);
            shadowQuad.localScale = new Vector3(scale, scale, scale);
        }
    }
}
