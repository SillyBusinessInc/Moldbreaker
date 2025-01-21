using UnityEngine;

public class MilkLake : MonoBehaviour
{
    [SerializeField] private float intervalSeconds = 1f;
    [SerializeField] private float addMoldPerInterval = 1f;
    private float timeSinceLastMold = 0f;

    public float buoyancyForce = 50f;
    
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = GlobalReference.GetReference<PlayerReference>().Player;
        // change the position and rotation to make it look like player is swimming
        var submergedDepth = Mathf.Max(0, this.transform.position.y - other.transform.position.y); // Depth below surface
        var buoyancy = Vector3.up * this.buoyancyForce * submergedDepth;
        player.rb.AddForce(buoyancy, ForceMode.Force);
        this.SetRotation(10f);

        this.timeSinceLastMold -= Time.deltaTime;
        if (!(this.timeSinceLastMold <= 0f)) return;
            
        player.OnHit(this.addMoldPerInterval, Vector3.zero, DamageCause.HAZARD);
        this.timeSinceLastMold = this.intervalSeconds;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        this.SetRotation(0f);
        this.timeSinceLastMold = 0f;
    }

    private void SetRotation(float rotation)
    {
        var reference = GlobalReference.GetReference<PlayerReference>();
        var playerModel = reference.playerBradleyWaffletail.transform;
        
        playerModel.rotation = Quaternion.Euler(
            rotation, 
            playerModel.rotation.eulerAngles.y,
            playerModel.rotation.eulerAngles.z
        );
    }
}
