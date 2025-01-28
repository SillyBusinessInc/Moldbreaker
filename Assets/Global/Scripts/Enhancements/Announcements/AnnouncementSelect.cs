using UnityEngine;
using UnityEngine.EventSystems;

public class AnnouncementSelect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isSelected;

    void Update()
    {
        if (isSelected)
        {
            if (EventSystem.current.currentSelectedGameObject != this.gameObject)
            {
                isSelected = false;
                animator.SetTrigger("deselected");
            }
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            isSelected = true;
            animator.SetTrigger("selected");
        }

    }
}
