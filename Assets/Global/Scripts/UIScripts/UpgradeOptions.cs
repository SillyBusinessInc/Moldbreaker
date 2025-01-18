using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UpgradeOptions : Reference
{
    [HideInInspector] public UpgradeOption option;
    public UpgradeOptionLogic UpgradeOptionLogic;
    [HideInInspector] public bool isShown = false;

    public List<ActionParamPair> interactionActions;
    [SerializeField] private Button confirmButton;
    [SerializeField] private UIInputHandler handler;

    protected new void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    [ContextMenu("SHOW")]
    public void ShowOption()
    {
        // handler.EnableInput("UI");
        isShown = true;
        GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
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
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        // handler.DisableInput("UI");
    }

    void SetCursorState(bool cursorVisible, CursorLockMode lockMode)
    {
        Cursor.visible = cursorVisible;
        Cursor.lockState = lockMode;
    }

    public void Confirm()
    {
        if (!isShown) return;

        // if (ctx.started && option != null) // ctx?? where is ctx?
        if (option != null)
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
        }
        AudioManager.Instance.PlaySFX("PowerupPickup");
        HideOption();
    }
}