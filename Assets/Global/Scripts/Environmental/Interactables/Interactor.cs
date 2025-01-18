using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayers;
    private Interactable currentInteractable;

    [Header("Raycast Settings [Debug]")]
    [SerializeField] private float rayDistance = 10f;         // Maximum detection distance   

    private bool isColliding = false;

    private void Update()
    {
        DetectInteractable();
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (currentInteractable != null && !currentInteractable.IsDisabled && ctx.started)
        {
            currentInteractable.TriggerInteraction(this);
        }
    }

    private void DetectInteractable()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 forward = transform.forward;

        // Check if the player is colliding with an interactable
        bool interactableFound = RaycastFindInteractable(origin, forward);

        // If no interactable was found, clear the current interactable
        if (!interactableFound && currentInteractable != null && !isColliding)
        {
            Debug.Log("Clearing current interactable");
            currentInteractable.ShowPrompt(false);
            currentInteractable = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (currentInteractable != null && currentInteractable != interactable)
            {
                currentInteractable.ShowPrompt(false);
            }

            isColliding = true;
            SetInteractable(interactable);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        var interactable = collision.collider.GetComponent<Interactable>();
        if (interactable != null && interactable == currentInteractable)
        {
            isColliding = false;
            currentInteractable.ShowPrompt(false);
            currentInteractable = null;
        }
    }

    private bool RaycastFindInteractable(Vector3 origin, Vector3 direction)
    {
        Vector3[] rayOffsets = { origin + Vector3.up * .75f, origin + Vector3.up * .5f, origin + Vector3.up * 0.1f };
        Collider[] colliders = new Collider[10];

        foreach (var rayOffset in rayOffsets)
        {
            Ray ray = new(rayOffset, direction);
            // First check for overlap sphere, to see if the start of the ray is already colliding with an interactable
            int amount = Physics.OverlapSphereNonAlloc(rayOffset, 0.1f, colliders, ~ignoreLayers);
            for (int i = 0; i < amount; i++)
            {
                Collider collider = colliders[i];
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    SetInteractable(interactable);
                    return true;
                }
            }

            // Debug visualization
            // Debug.DrawRay(rayOffset, direction * rayDistance, Color.green, 0.1f);

            // Then check for raycast hit
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, ~ignoreLayers))
            {
                if(hit.collider.TryGetComponent(out Interactable interactable)) {
                    float rayHitDistance = Vector3.Distance(transform.position, hit.point);

                    if (interactable.IsWithinInteractionRange(rayHitDistance))
                    {
                        SetInteractable(interactable);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SetInteractable(Interactable interactable)
    {
        if (currentInteractable != interactable)
        {
            if (currentInteractable != null) currentInteractable.ShowPrompt(false);
            currentInteractable = interactable;
            currentInteractable.ShowPrompt(true);
        }
    }
}