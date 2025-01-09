using System;
using UnityEngine;

public class MilkLake : MonoBehaviour
{
    [SerializeField] private float intervalSeconds = 1f;
    [SerializeField] private float addMoldPerInterval = 1f;
    private float timeSinceLastMold = 0f;

    private GameObject playerObject;
    private Transform playerObjectTransform;

    private void Start()
    {
        playerObject = GlobalReference.GetReference<PlayerReference>().playerBradleyWaffletail;
        playerObjectTransform = playerObject.transform;

        // duplicate the gameobject to make player float on top of lake
        if (!gameObject.CompareTag("Water")) { 
            // the gameobjects without the water tag needs to have a duplicated lake to make sure player looks like it can float
            GameObject childObject = Instantiate(gameObject);
            MilkLake milkLakeScript = childObject.GetComponent<MilkLake>();
            if (milkLakeScript != null) {
                Destroy(milkLakeScript); // Remove the script
            }
            childObject.transform.parent = gameObject.transform;
            childObject.transform.localPosition = new Vector3(0, -0.1f, 0);

            MeshCollider meshCollider = childObject.GetComponent<MeshCollider>();
            if (meshCollider != null) {
                meshCollider.isTrigger = false;
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null) {
                // change the position and rotation to make it look like player is swimming
                playerObjectTransform.localPosition = new Vector3(0f, -1f, 0f);
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
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // reset the position and rotation
            playerObjectTransform.localPosition = new Vector3(0, 0, 0);
            playerObjectTransform.rotation = Quaternion.Euler(
                0f,
                playerObjectTransform.rotation.eulerAngles.y, 
                playerObjectTransform.rotation.eulerAngles.z
            );
            timeSinceLastMold = 0f;
        }
    }
}
