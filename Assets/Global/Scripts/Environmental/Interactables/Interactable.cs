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

    private string cachedEnabledPrompt;
    private string cachedDisabledPrompt;

    [Header("HUD Settings")]
    [SerializeField]
    [Range(-10f, 10f)]
    private float promptYOffset = 1.5f;
    [SerializeField] private Color enabledPromptColor = Color.white;
    [SerializeField] private Color disabledPromptColor = Color.white;

    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private int promptFontSize = 8;

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

    [SerializeField]
    private ControlIconMapping controlIconMappingConfig;
    private string lastSavedControlScheme;


    public virtual void Start()
    {
        playerCamera = GlobalReference.GetReference<PlayerReference>().PlayerCamera;
        playerInput = GlobalReference.GetReference<PlayerReference>().Player.GetComponent<PlayerInput>();

        // Create a HUD element to display the interaction prompt
        if (hudElement == null) InstantiateHUD();

        IsDisabled = isDisabled; 
        GlobalReference.SubscribeTo(Events.DEVICE_CHANGED, () => SetBillboardText(true));
    } 

    private IconPathResult ParseDeviceInputSprite()
    {
        if (!playerInput) return null;

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
                return HandleControllerInput(TDeviceType.XboxController, controlPath);
            }

        }
        else
        {
            return HandleControllerInput(TDeviceType.Keyboard, controlPath);
        }

        IconPathResult HandleControllerInput(TDeviceType deviceType, string controlPath) => controlIconMappingConfig.GetIcon(deviceType, controlPath);

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

        if (font) hudText.font = font;

        // set right coordinates
        hudElement.transform.SetParent(hudParent != null ? hudParent : transform);

        SetBillboardText(true);
        // set offsets & colors etc
        SetHUDSettings();

        hudElement.SetActive(false);
    }

    [ContextMenu("Force update HUD settings")]
    private void SetHUDSettings()
    {
        hudText.alignment = TextAlignmentOptions.Center;
        hudText.fontSize = promptFontSize;
        hudText.color = isDisabled ? disabledPromptColor : enabledPromptColor;

        // set offsets
        hudElement.transform.position = hudParent != null ? hudParent.position : transform.position;
        hudElement.transform.position += Vector3.up * promptYOffset;
        hudElement.transform.position += Vector3.right * promptXOffset;
        hudElement.transform.position += Vector3.forward * promptZOffset;

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
        ActionMetaData metaData = new(interactor.gameObject, gameObject);
        if (!isDisabled)
        {
            OnInteract(metaData);
        }
        else
        {
            OnFailedInteract();
            failedInteractionActions.Invoke();
        }
    }

    private void SetBillboardText(bool regenerate = false)
    {
        Debug.Log("SetBillboardText");
        if (hudElement == null || hudText == null) return;

        // if device didn't change, just return the cached string
        if (!regenerate)
        {
            hudText.text = isDisabled ? cachedDisabledPrompt : cachedEnabledPrompt;
            return;
        }

        string enabledParsedString = getParsedString(interactionPrompt);
        string disabledParsedString = getParsedString(disabledPrompt);

        hudText.text = isDisabled ? disabledParsedString : enabledParsedString;

        hudText.color = isDisabled ? disabledPromptColor : enabledPromptColor;

        cachedDisabledPrompt = disabledParsedString;
        cachedEnabledPrompt = enabledParsedString;

        if (string.IsNullOrEmpty(hudElement.GetComponent<TextMeshPro>().text))
        {
            hudElement.SetActive(false);
        }
    }

    string getParsedString(string str)
    {
        // check if string contains {interactKey}
        if (str.Contains("{interactKey}"))
        {
            IconPathResult iconInfo = ParseDeviceInputSprite();

            if (iconInfo == null || iconInfo.icon == null)
            {
                return str.Replace("{interactKey}", playerInput.actions["Interact"].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions));
            }
            else
            {
                hudText.spriteAsset = iconInfo.icon;
                return str.Replace("{interactKey}", "<sprite=" + iconInfo.index + ">");
            }
        }

        return str;
    }
}
