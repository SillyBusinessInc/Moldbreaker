using UnityEngine;
using UnityEngine.InputSystem;
public class UIInputHandler : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputActionMap> ActionMap;
    private InputActionMap UIActionMap;
    [SerializeField] private UpgradeOptions upgradeOptions;

    public void OnConfirm(InputAction.CallbackContext ctx)
    {
        upgradeOptions.Confirm(ctx);
    }
    public void EnableInput(string mapName)
    {
        // enable UI actionmap and disable all other actionmap
        // to make sure at this moment you can only use the UI actionmap
        foreach (var actionMap in ActionMap)
        {
            if (actionMap.name == mapName)
            {
                UIActionMap.Enable();
            }
            else
            {
                actionMap.Disable();
            }
        }
    }

    public void DisableInput(string mapName)
    {
        // disable ui actionmap and enable the rest
        foreach (var actionMap in ActionMap)
        {
            if (actionMap.name == mapName)
            {
                UIActionMap.Disable();
            }
            else
            {
                actionMap.Enable();
            }
        }
    }

}