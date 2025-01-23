using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHoverLogic : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Underline;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Normal;
    }
}
