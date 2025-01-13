using System;
using UnityEngine;

public class MilkLake : MonoBehaviour
{
    [SerializeField] private float intervalSeconds = 1f;
    [SerializeField] private float addMoldPerInterval = 1f;
    private float timeSinceLastMold = 0f;

    private GameObject waffleTailObject;
    private Transform playerObjectTransform;

    private void Start()
    {
        waffleTailObject = GlobalReference.GetReference<PlayerReference>().playerBradleyWaffletail;
        playerObjectTransform = waffleTailObject.transform;

        // duplicate the gameobject to make player float on top of lake
        GameObject childObject = Instantiate(gameObject);
        MilkLake milkLakeScript = childObject.GetComponent<MilkLake>();
        if (milkLakeScript != null) {
            Destroy(milkLakeScript); // Remove the script
        }
        childObject.transform.parent = gameObject.transform;
        if (!gameObject.CompareTag("Water")) { 
            childObject.transform.localPosition = new Vector3(0, -0.1f, 0);
        } else {
            childObject.transform.localPosition = new Vector3(0, 0.3f, 0);
        }

        MeshCollider meshCollider = childObject.GetComponent<MeshCollider>();
        if (meshCollider != null) {
            meshCollider.isTrigger = false;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            // change the position and rotation to make it look like player is swimming
            playerObjectTransform.rotation = Quaternion.Euler(
                10f,
                playerObjectTransform.rotation.eulerAngles.y, 
                playerObjectTransform.rotation.eulerAngles.z
            );

            // will add mold every second
            timeSinceLastMold -= Time.deltaTime;
            if (timeSinceLastMold <= 0f) {
                Player player = GlobalReference.GetReference<PlayerReference>().Player;
                player.OnHit(addMoldPerInterval, Vector3.zero);
                timeSinceLastMold = intervalSeconds;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // reset the position and rotation
            playerObjectTransform.rotation = Quaternion.Euler(
                0f,
                playerObjectTransform.rotation.eulerAngles.y, 
                playerObjectTransform.rotation.eulerAngles.z
            );
            timeSinceLastMold = 0f;
        }
    }
}
