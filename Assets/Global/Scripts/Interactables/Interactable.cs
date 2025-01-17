using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "E - Interact";
    [SerializeField] private string disabledPrompt = "Cannot interact";

    [Header("HUD Settings")]
    [SerializeField]
    [Range(-10f, 10f)]
    private float promptYOffset = 1.5f;

    [SerializeField] private float promptXOffset = 0.0f;
    [SerializeField] private float promptZOffset = 0.0f;

    [Tooltip("The parent object that the HUD will be attached to, if not set it will be attached to the interactable object")]
    [SerializeField] private Transform hudParent;

    [Range(0, 10)]
    [SerializeField] private float interactDistance = 5.0f;


    [Header("Initial State")]
    [SerializeField]
    private bool isDisabled = false;
    private Camera playerCamera;
    private GameObject hudElement;
    

    // list of scriptable objects that will be invoked when the interactable is triggered 
    [Header("Actions")]
    [SerializeField] private List<ActionParamPair> interactionActions;
    [SerializeField] private UnityAction failedInteractionActions;
    public bool IsDisabled
    {
        get => isDisabled;
        set
        {
            isDisabled = value;
            SetBillboardText();

            if (isDisabled)
            {
                OnDisableInteraction();
            }

            if (!isDisabled)
            {
                OnEnableInteraction();
            }
        }
    }

    public virtual void Start()
    {
        playerCamera = GlobalReference.GetReference<PlayerReference>().PlayerCamera;

        // Create a HUD element to display the interaction prompt
        if (hudElement == null) InstantiateHUD();

        IsDisabled = isDisabled;
    }

    public virtual void OnInteract(ActionMetaData metaData)
    {
        // loop over the list of actions and invoke them
        interactionActions.ForEach(action => action.InvokeAction(metaData));
    }

    public virtual void OnFailedInteract() {}

    public virtual void OnEnableInteraction() {}

    public virtual void OnDisableInteraction() {}

    private void InstantiateHUD()
    {
        // Create a HUD element to display the interaction prompt
        hudElement = new GameObject("HUDPrompt");
        hudElement.AddComponent<TextMesh>().text = interactionPrompt;
        hudElement.transform.localScale = Vector3.one * 0.2f;
        hudElement.SetActive(false);
        hudElement.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;

        // set offsets
        hudElement.transform.position = transform.position + Vector3.up * promptYOffset;
        hudElement.transform.position += Vector3.right * promptXOffset;
        hudElement.transform.position += Vector3.forward * promptZOffset;

        // set right coordinates
        if (hudParent != null)
            hudElement.transform.SetParent(hudParent);
        else
            hudElement.transform.SetParent(transform);
    }

    public bool IsWithinInteractionRange(float rayHitDistance) => rayHitDistance <= interactDistance;

    public void ShowPrompt(bool show)
    {
        if (hudElement == null)
        {
            Debug.Log("[Improper Configuration] No HUD element found, make sure the inherited class calls the base.Start() method [Interactable.cs]");
            return;
        }

        hudElement.SetActive(show);
        if (show)
        {
            hudElement.transform.position = transform.position;
            RotateBillboardTowardsCamera();
            SetBillboardText();
        }
    }

    private void Update()
    {
        RotateBillboardTowardsCamera();
    }

    private void RotateBillboardTowardsCamera()
    {
        if (hudElement == null || playerCamera == null) return;
        Vector3 directionToCamera = playerCamera.transform.position - hudElement.transform.position;
        hudElement.transform.rotation = Quaternion.LookRotation(-directionToCamera);
    }

    public virtual void TriggerInteraction(PlayerInteraction interactor)
    {
        ActionMetaData metaData = new(interactor.gameObject, this.gameObject);
        if (!isDisabled)
        {
            OnInteract(metaData);
            // Invoke all actions
            interactionActions.ForEach(action => action.InvokeAction(metaData));
        }
        else
        {
            OnFailedInteract();
            failedInteractionActions.Invoke();
        }
    }

    private void SetBillboardText()
    {
        if (hudElement == null) return;

        hudElement.GetComponent<TextMesh>().text = isDisabled ? disabledPrompt : interactionPrompt;

        if (string.IsNullOrEmpty(hudElement.GetComponent<TextMesh>().text))
        {
            hudElement.SetActive(false);
        }
    }
}
