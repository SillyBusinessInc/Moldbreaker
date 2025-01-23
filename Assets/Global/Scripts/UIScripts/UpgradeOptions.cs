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

    public void ShowOption()
    {
        isShown = true;
        GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_ACTIVE);
        UILogic.SelectButton(confirmButton);
        UILogic.ShowCursor();
        Time.timeScale = 0;
        gameObject.SetActive(true);

        if (option != null)
        {
            UpgradeOptionLogic.data = option;
            UpgradeOptionLogic.SetData();
        }
    }

    public void HideOption()
    {
        Time.timeScale = 1;
        UILogic.HideCursor();
        gameObject.SetActive(false);
        isShown = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
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