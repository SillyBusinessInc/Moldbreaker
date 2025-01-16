using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using static ControlIconMapping;
using TMPro;
using UnityEngine.InputSystem.XInput;
using System;



public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "{interactKey} - Interact";
    [SerializeField] private string disabledPrompt = "Cannot interact";
    private string cachedInteractionPrompt; 

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
    private TMP_Text hudText;

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

    private PlayerInput playerInput;
    private string currentControlDevice;

    [SerializeField]
    private ControlIconMapping controlIconMappingConfig;


    public virtual void Start()
    {

        playerCamera = GlobalReference.GetReference<PlayerReference>().PlayerCamera;

        // get playerInput
        playerInput = GlobalReference.GetReference<PlayerReference>().Player.GetComponent<PlayerInput>();
      
        // Create a HUD element to display the interaction prompt
        if (hudElement == null) InstantiateHUD(); 

        IsDisabled = isDisabled; 
    }

    private IconPathResult ParseDeviceInputSprite()
    {  

        if (playerInput)
        {
            string controlPath = playerInput.actions["Interact"].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);

            // get the device the player is using
            string deviceLayout = playerInput.currentControlScheme;

            // get if you are on an xbox or playstation controller

            if (Gamepad.current != null && deviceLayout == "Gamepad")
            {
                var gamepad = Gamepad.current;

                // Check if the gamepad is a DualShock controller (PlayStation controller)
                if (gamepad is DualShockGamepad)
                {
                    Debug.Log("DualShockGamepad");
                   return HandleControllerInput(TDeviceType.PlayStationController, controlPath);
                }
                // Check if the gamepad is an Xbox controller
                else if (gamepad is XInputController)
                {
                    Debug.Log("XboxController");
                  return  HandleControllerInput(TDeviceType.XboxController, controlPath);
                } 

            }
            else
            { 
                return HandleControllerInput(TDeviceType.Keyboard, controlPath);
            }

             IconPathResult HandleControllerInput(TDeviceType deviceType, string controlPath) => controlIconMappingConfig.GetIcon(deviceType, controlPath);
        }

        return null;
    }

    public virtual void OnInteract(ActionMetaData metaData)
    {
        // loop over the list of actions and invoke them
        interactionActions.ForEach(action => action.InvokeAction(metaData));
    }

    public virtual void OnFailedInteract() { }

    public virtual void OnEnableInteraction() { }

    public virtual void OnDisableInteraction() { }

    private void InstantiateHUD()
    {
        // Create a HUD element to display the interaction prompt
        hudElement = new GameObject("HUDPrompt");
        hudElement.AddComponent<TextMeshPro>().text = interactionPrompt;

        hudText = hudElement.GetComponent<TextMeshPro>(); 

        hudElement.transform.localScale = Vector3.one * 0.2f;
        hudElement.SetActive(false);
        hudElement.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;

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

        public void OnControlsChanged()
        {
          SetBillboardText(true);

        }


    private void Update()
    {
        RotateBillboardTowardsCamera();

        // get current active control device
        string currentDevice = playerInput.currentControlScheme;
        if (currentControlDevice != currentDevice)
        {
            currentControlDevice = currentDevice;
            
        }
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

    private void SetBillboardText(bool ?regenerate = false)
    {
        if (hudElement == null) return;

        string baseString = isDisabled ? disabledPrompt : interactionPrompt;
        string parsedString;
 
        // check if string contains {interactKey}
        if (baseString.Contains("{interactKey}")) {

        IconPathResult iconInfo = ParseDeviceInputSprite(); 

        if (iconInfo == null) return;

        parsedString = baseString.Replace("{interactKey}", "<sprite=" + iconInfo.index + ">");
        cachedInteractionPrompt = parsedString;
        hudText.text = parsedString;
        } else {
            hudText.text = disabledPrompt;
        }
 
        if (string.IsNullOrEmpty(hudElement.GetComponent<TextMeshPro>().text))
        {
            hudElement.SetActive(false);
        }
    }
}
