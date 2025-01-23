using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    private TMP_Text title;
    private TMP_Text description;
    private Action confirmAction;
    private Action rejectAction;
    [SerializeField] private CanvasGroup menuGroup;
    [SerializeField] private CanvasGroup confirmationGroup;
    [SerializeField] private Button noButton;
    private GameObject origin;

    public void RequestConfirmation(string title_, string description_, Action confirmAction_, GameObject origin_, Action rejectAction_ = null)
    {
        gameObject.SetActive(true);
        title = transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetComponent<TMP_Text>();

        title.text = title_;
        description.text = description_;
        confirmAction = confirmAction_;
        rejectAction = rejectAction_;
        origin = origin_;

        // make confirmation window the only interactable menu, and set no-button as selected option.
        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(noButton);
    }

    public void OnConfirm()
    {
        confirmAction?.Invoke();
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        gameObject.SetActive(false);
        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(origin);
    }

    public void OnReject()
    {
        rejectAction?.Invoke();
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        gameObject.SetActive(false);

        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(origin);
    }
}
