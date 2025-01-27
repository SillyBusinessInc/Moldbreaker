using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptions : Reference
{
    [HideInInspector] public UpgradeOption option;
    public UpgradeOptionLogic UpgradeOptionLogic;
    [HideInInspector] public bool isShown = false;

    public List<ActionParamPair> interactionActions;
    [SerializeField] private Button confirmButton;

    protected new void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void Start()
    {
        GlobalReference.SubscribeTo(Events.DEVICE_CHANGED, OnDeviceChanged);
    }

    private void SetUIState(bool value)
    {
        isShown = value;
        gameObject.SetActive(value);
                
        UILogic.SetCursor(value && UILogic.GetInputType() == "keyboard");
        Time.timeScale = value ? 0f : 1f;
        GlobalReference.AttemptInvoke(value ? Events.INPUT_IGNORE : Events.INPUT_ACKNOWLEDGE);
    }
    
    private void OnDeviceChanged()
    {
        if(isShown)
            UILogic.SetCursor(UILogic.GetInputType() == "keyboard");
    }

    public void ShowOption()
    {
        SetUIState(true);
        UILogic.SelectButton(confirmButton);
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_ACTIVE);

        if (option != null)
        {
            UpgradeOptionLogic.data = option;
            UpgradeOptionLogic.SetData();
        }
    }

    public void HideOption()
    {
        SetUIState(false);
    }

    public void Confirm()
    {
        if (!isShown) return;
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
        GlobalReference.GetReference<AudioManager>().PlaySFX("PowerupPickup");
        HideOption();
    }
}