using UnityEngine;
using System.Collections;

public class BreakableObjectWithAnimation : MonoBehaviour
{
    [SerializeField] private float delay = 3f;     
    [SerializeField] private float respawnDelay = 7f; 

    [SerializeField] private bool canRespawn = true;
    [SerializeField] private bool canBreak = true; 

    private Animator animator;
    private Collider boxCollider;
    private Renderer platformRenderer;
    private PlayerObject playerObject;

    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision other) {
        animator.ResetTrigger("respawn");
        playerObject = GlobalReference.GetReference<PlayerReference>().PlayerObj;

        if (other.gameObject == playerObject.gameObject && canBreak) {
            Invoke(nameof(CrackObject), delay);
        }
    }

    private void CrackObject()
    {
        animator.SetTrigger("cracking");
        Invoke(nameof(RespawnObject), respawnDelay);
    }


    private void RespawnObject()
    {
        animator.ResetTrigger("cracking");

        if (!canRespawn) {
            animator.enabled = false;
            boxCollider.enabled = false;
            platformRenderer.enabled = false;
        } else {
            animator.SetTrigger("respawn");
        }

    }
}
