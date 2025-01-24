using UnityEngine;
using TMPro;

public class SpeedrunMode : MonoBehaviour
{
    private float currentTime = 0f;
    private float currentTimePerLevel = 0f;
    [SerializeField] private TMP_Text timerText;

    void Start()
    {
        GlobalReference.SubscribeTo(Events.SPEEDRUN_MODE_TOGGLED, () => ToggleMode());
        GlobalReference.SubscribeTo(Events.ROOM_FINISHED, () => SaveTimeCurrentLevel());
        ToggleMode();

        // needed for example when you die, the timer needs to be set the same value as before you died
        currentTime = ParseTimerText(GlobalReference.Statistics.Get<string>("total_time"));
    }

    void Update()
    {
        if (GlobalReference.GetReference<GameManagerReference>().speedrunTimerRun) {
            currentTimePerLevel += Time.unscaledDeltaTime;
            currentTime += Time.unscaledDeltaTime;
            timerText.text = GetTimerText(currentTime);
        }
    }

    void ResetTimerPerLevel() {
        currentTimePerLevel = 0;
    }

    void SaveTimeCurrentLevel() {
        if (GlobalReference.GetReference<GameManagerReference>().speedrunTimerRun) {
            string timeOfLevel = GetTimerText(currentTimePerLevel);
            switch (GlobalReference.GetReference<GameManagerReference>().activeRoom.id) {
                case 1:
                    GlobalReference.Statistics.Set("level_1_time", timeOfLevel);
                    break;
                case 2:
                    GlobalReference.Statistics.Set("level_2_time", timeOfLevel);
                    break;
                case 3:
                    GlobalReference.Statistics.Set("level_3_time", timeOfLevel);
                    GlobalReference.Statistics.Set("total_time", GetTimerText(currentTime));
                    break;
                default: // when player is in hub it has to reset
                    ResetTimerPerLevel();
                    GlobalReference.Statistics.Set("total_time", GetTimerText(currentTime));
                    break;
            }
            GlobalReference.Statistics.SaveAll();
        }
    }

    string GetTimerText(float time) {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int hundredths = Mathf.FloorToInt((time * 100) % 100);

        return $"{minutes:00}:{seconds:00}:{hundredths:00}";
    }

    float ParseTimerText(string timerText) {
        string[] parts = timerText.Split(':');
        
        if (parts.Length != 3)
            throw new System.FormatException("Invalid time format. Expected MM:SS:HH");

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        int hundredths = int.Parse(parts[2]);

        return (minutes * 60) + seconds + (hundredths / 100f);
    }

    void ToggleMode() {
        var speedrun_active = GlobalReference.Settings.Get<bool>("speedrun_mode");
        gameObject.SetActive(speedrun_active);
    }
}