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
    [SerializeField] private Button quitButton;

    void Start()
    {
        // title = transform.GetChild(0).GetComponent<TMP_Text>();
        // description = transform.GetChild(1).GetComponent<TMP_Text>();

        // gameObject.SetActive(false);
    }

    public void RequestConfirmation(string title_, string description_, Action confirmAction_, Action rejectAction_ = null)
    {
        gameObject.SetActive(true);
        title = transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).GetComponent<TMP_Text>();

        title.text = title_;
        description.text = description_;
        confirmAction = confirmAction_;
        rejectAction = rejectAction_;

        // make confirmation window the only interactable menu, and set no-button as selected option.
        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(noButton);
    }

    public void OnConfirm()
    {
        if (confirmAction != null) confirmAction();
        AudioManager.Instance.PlaySFX("Button");
        gameObject.SetActive(false);
        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(quitButton);
    }

    public void OnReject()
    {
        if (rejectAction != null) rejectAction();
        AudioManager.Instance.PlaySFX("Button");
        gameObject.SetActive(false);
        UILogic.FlipInteractability(confirmationGroup, menuGroup);
        UILogic.SelectButton(quitButton);
    }
}
