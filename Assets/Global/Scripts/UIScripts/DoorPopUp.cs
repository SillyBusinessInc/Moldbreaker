using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorPopUp : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string popupTitle;
    [SerializeField] private string maxCrumbs;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI crumbs;
    [SerializeField] private List<RawImage> calories;
    private CollectableSave saveData;
    [SerializeField] private Animator animator;
    private bool isPopupVisible = false;

    void Start()
    {
        saveData = new CollectableSave(levelName);
        saveData.LoadAll();
        title.text = popupTitle;
        crumbs.text = saveData.Get<int>("crumbs") + "/" + maxCrumbs;

        List<string> savedCaloriesTrimmed = 
        saveData.Get<List<string>>("calories")
        .Select(x => x.Split(new[] { ">>>UNIQUE_DELIMITER>>>" }, StringSplitOptions.None)
        .LastOrDefault()).ToList(); 

        UpdateCaloriesDisplay(savedCaloriesTrimmed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPopupVisible)
        {
            isPopupVisible = true; // Mark popup as visible
            animator.SetTrigger("open");
            GlobalReference.GetReference<PlayerReference>().Player.setCameraHeight(10f);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPopupVisible)
        {
            isPopupVisible = false; // Mark popup as not visible
            animator.SetTrigger("close");
            GlobalReference.GetReference<PlayerReference>().Player.setCameraHeight(null); // height reset to default
        }
    }

    private void UpdateCaloriesDisplay(List<string> savedCalories)
    {
        foreach (var image in calories)
        {
            string progressionOrder = ExtractProgressionOrder(image);
            if (!savedCalories.Contains(progressionOrder))
            {
                SetImageAlpha(image, 0.5f); // Set alpha to 50%
            }
            else
            {
                SetImageAlpha(image, 1f); // Ensure collected items are fully visible
            }
        }
    }

    private string ExtractProgressionOrder(RawImage image)
    {
        // Extracting progression order logic
        string progressionOrder = image.gameObject.name.Split(new[] { "Order_" }, StringSplitOptions.None)[1];
        return progressionOrder;
    }

    private void SetImageAlpha(RawImage image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
