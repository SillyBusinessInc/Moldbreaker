using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeOptions : Reference
{
    [HideInInspector] public UpgradeOption option;
    [SerializeField] private UpgradeList UpgradesUIList;
    public UpgradeOptionLogic UpgradeOptionLogic;
    [HideInInspector] public bool isShown = false;

    public InputActionAsset inputActionAsset;
    private UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputActionMap> ActionMap;
    private InputActionMap UIActionMap;

    public List<ActionParamPair> interactionActions;
    [SerializeField] private Button confirmButton;

    protected new void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);

        ActionMap = inputActionAsset.actionMaps;
        foreach (var actionMap in ActionMap)
        {
            if (actionMap.name == "UI")
            {
                UIActionMap = actionMap;
            }
        }

        DisableUIInput();
    }

    [ContextMenu("SHOW")]
    public void ShowOption()
    {
        EnableUIInput();
        isShown = true;
        UILogic.SelectButton(confirmButton);
        SetCursorState(true, CursorLockMode.None);
        Time.timeScale = 0;
        gameObject.SetActive(true);

        if (option != null)
        {
            UpgradeOptionLogic.data = option;
            UpgradeOptionLogic.SetData();
        }
    }

    [ContextMenu("HIDE")]
    public void HideOption()
    {
        Time.timeScale = 1;
        SetCursorState(false, CursorLockMode.Locked);
        gameObject.SetActive(false);
        isShown = false;
        DisableUIInput();
    }

    void SetCursorState(bool cursorVisible, CursorLockMode lockMode)
    {
        Cursor.visible = cursorVisible;
        Cursor.lockState = lockMode;
    }

    public void Confirm(InputAction.CallbackContext ctx)
    {
        if (!isShown) return;

        if (ctx.started && option != null)
        {
            foreach (ActionParamPair action in option.interactionActions)
            {
                action.InvokeAction();
            }

            foreach (ActionParamPair action in interactionActions)
            {
                action.InvokeAction();
                GlobalReference.AttemptInvoke(Events.STATISTIC_CHANGED);
            }

            UpgradesUIList.AddUpgrade(option);
        }
        HideOption();
    }

    void EnableUIInput()
    {
        // enable UI actionmap and disable all other actionmap
        // to make sure at this moment you can only use the UI actionmap
        foreach (var actionMap in ActionMap)
        {
            if (actionMap.name == "UI")
            {
                UIActionMap.Enable();
            }
            else
            {
                actionMap.Disable();
            }
        }
    }

    void DisableUIInput()
    {
        // disable ui actionmap and enable the rest
        foreach (var actionMap in ActionMap)
        {
            if (actionMap.name == "UI")
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