using UnityEngine;
using TMPro;

public class SpeedrunMode : MonoBehaviour
{
    private float currentTime = 0f;
    [SerializeField] private TMP_Text timerText;

    void Start()
    {
        GlobalReference.SubscribeTo(Events.SPEEDRUN_MODE_TOGGLED, () => ToggleMode());
        ToggleMode();
    }

    void Update()
    {
        if (GlobalReference.GetReference<GameManagerReference>().speedrunTimerRun) {
            currentTime += Time.unscaledDeltaTime;
            SetTimerText();
        }
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }

    void SetTimerText() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int hundredths = Mathf.FloorToInt((currentTime * 100) % 100);

        timerText.text = $"{minutes:00}:{seconds:00}:{hundredths:00}";
    }

    void ToggleMode() {
        var speedrun_active = GlobalReference.Settings.Get<bool>("speedrun_mode");
        gameObject.SetActive(speedrun_active);
    }
}