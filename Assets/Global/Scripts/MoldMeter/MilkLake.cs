using UnityEngine;

public class MilkLake : MonoBehaviour
{
    [SerializeField] private float intervalSeconds = 1f;
    [SerializeField] private float addMoldPerInterval = 1f;
    private float timeSinceLastMold = 0f;

    private GameObject waffleTailObject;
    private Rigidbody rb;
    private Transform waffleTailObjectTransform;

    public float buoyancyForce = 50f;

    private void Start()
    {
        waffleTailObject = GlobalReference.GetReference<PlayerReference>().playerBradleyWaffletail;
        rb = GlobalReference.GetReference<PlayerReference>().PlayerObj.GetComponent<Rigidbody>();
        waffleTailObjectTransform = waffleTailObject.transform;
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            // change the position and rotation to make it look like player is swimming
            float submergedDepth = Mathf.Max(0, transform.position.y - other.transform.position.y); // Depth below surface
            Vector3 buoyancy = Vector3.up * buoyancyForce * submergedDepth;
            rb.AddForce(buoyancy, ForceMode.Force);
            waffleTailObjectTransform.rotation = Quaternion.Euler(
                10f, 
                waffleTailObjectTransform.rotation.eulerAngles.y,
                waffleTailObjectTransform.rotation.eulerAngles.z
            );


            // will add mold every second
            timeSinceLastMold -= Time.deltaTime;
            if (timeSinceLastMold <= 0f) {
                Player player = GlobalReference.GetReference<PlayerReference>().Player;
                player.lastDamageCause = Player.DamageCause.HAZARD;
                player.OnHit(addMoldPerInterval, Vector3.zero);
                timeSinceLastMold = intervalSeconds;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // reset the position and rotation
            waffleTailObjectTransform.rotation = Quaternion.Euler(
                0f, 
                waffleTailObjectTransform.rotation.eulerAngles.y,
                waffleTailObjectTransform.rotation.eulerAngles.z
            );
            timeSinceLastMold = 0f;
        }
    }
}
