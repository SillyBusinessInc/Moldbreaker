using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using static ControlIconMapping;

public class UpgradeOptionLogic : MonoBehaviour
{
    public UpgradeOption data;
    public UnityEngine.UI.Image image;
    [HideInInspector] public TMP_Text upgradeName;
    [HideInInspector] public TMP_Text description;
    public TMP_Text text1;
    public TMP_Text text2;
    public UnityEngine.UI.Image keyboardImage;
    private PlayerInput playerInput;
    [SerializeField] private GameObject PressKeyboard;

    [SerializeField] private ControlIconMapping controlIconMappingConfig;

    void Start()
    {
        playerInput = GlobalReference.GetReference<PlayerReference>().Player.GetComponent<PlayerInput>();

        upgradeName = transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetComponent<TMP_Text>();
        SetData();
    }

    private void Update()
    {
        text1.fontSize = description.fontSize;
        text2.fontSize = description.fontSize;
    }
    
    public void SetData()
    {
        image.sprite = data.image;
        upgradeName.text = data.name;
        description.text = data.description ?? "Hmm yes, yeast of power. So powerful";
        text1.text = data.text1;
        text2.text = data.text2;
        // keyboardImage.sprite = data.keyboardImage;

        playerInput = GlobalReference.GetReference<PlayerReference>().Player.GetComponent<PlayerInput>();

        string deviceLayout = playerInput.currentControlScheme;
        string controlPath = playerInput.actions[data.interactionKey].GetBindingDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);

        // check if there is a |, because then there are multiple bindings
        if (controlPath.Contains("|"))
        {
            controlPath = controlPath.Split('|')[0];
        }

        IconPathResult res = null;
        // get if you are on an xbox or playstation controller 

        if (Gamepad.current != null && deviceLayout == "Gamepad")
        {
            var gamepad = Gamepad.current;

            // Check if the gamepad is a DualShock controller (PlayStation controller)
            if (gamepad is DualShockGamepad)
            { 
                res = HandleControllerInput(TDeviceType.PlayStationController, controlPath);
            }
            // Check if the gamepad is an Xbox controller
            else if (gamepad is XInputController)
            {

                res = HandleControllerInput(TDeviceType.XboxController, controlPath);
            }

        }
        else
        {
            res = HandleControllerInput(TDeviceType.Keyboard, controlPath);
        }

        IconPathResult HandleControllerInput(TDeviceType deviceType, string controlPath) => controlIconMappingConfig.GetIcon(deviceType, controlPath);

        if (res != null && res.sprite) keyboardImage.sprite = res.sprite;
        
        if (text2.text == "")
        {
            RectTransform rt = text1.GetComponent<RectTransform>();
            rt.anchorMin = new(0f, 0);
            rt.anchorMax = new(0.325f, 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
            rt = keyboardImage.GetComponent<RectTransform>();
            rt.anchorMin = new(0.39f, 0);
            rt.anchorMax = new(0.91f, 1);
            rt.offsetMin = new(0f, 0f);
            rt.offsetMax = new(0f, 0f);
        }
    }
}