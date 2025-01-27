using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedrunResultLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    [SerializeField] TMP_Text level1Text;
    [SerializeField] TMP_Text level2Text;
    [SerializeField] TMP_Text level3Text;
    [SerializeField] TMP_Text totalTimeText;
    [SerializeField] TMP_Text totalDeathsText;

    void Start()
    {
        GlobalReference.Settings.LoadAll();
        SetTimes();
        UILogic.ShowCursor();
    }

    void SetTimes() {
        level1Text.text = GlobalReference.Statistics.Get<string>("level_1_time");
        level2Text.text = GlobalReference.Statistics.Get<string>("level_2_time");
        level3Text.text = GlobalReference.Statistics.Get<string>("level_3_time");
        totalTimeText.text = GlobalReference.Statistics.Get<string>("total_time");
        totalDeathsText.text = GlobalReference.Statistics.Get<int>("deaths").ToString();
    }

    public void OnExit() 
    {
        UILogic.FadeToScene("Menu", fadeImage, this);
        ResetTimers();
    }

    void ResetTimers() {
        GlobalReference.Statistics.Set("level_1_time", "00:00:00");
        GlobalReference.Statistics.Set("level_2_time", "00:00:00");
        GlobalReference.Statistics.Set("level_3_time", "00:00:00");
        GlobalReference.Statistics.Set("total_time", "00:00:00");
        GlobalReference.Statistics.Set("deaths", 0);

        GlobalReference.Statistics.SaveAll();
    }
}
