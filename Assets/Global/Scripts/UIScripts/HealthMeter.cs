using UnityEngine;
using TMPro;
using System.Collections;

public class HealthMeter : MonoBehaviour
{
    private Player player;
    [SerializeField] private TMP_Text HealthPercentageText;
    [SerializeField] private float healthPercentage;
    [SerializeField] private RectTransform HealthMeterImage;
    private Coroutine moveCoroutine;
    [SerializeField] private float animationDuration = 0.5f; // Duration for smooth movement
    private float savedHealthPercentage = -1;

    void Awake()
    {
        player = GlobalReference.GetReference<PlayerReference>().Player;
        GlobalReference.SubscribeTo(Events.MOLDMETER_CHANGED, UpdateHealthMeter);
    }

    void Update() => UpdateHealthMeter();

    public void UpdateHealthMeter()
    {
        healthPercentage = player.playerStatistic.Health/6*100;
        // if (moldPercentage < 100) moldPercentage += 0.01f;
        if (savedHealthPercentage == healthPercentage) return;
        savedHealthPercentage = healthPercentage;
        
        string decimals = healthPercentage >= 100 || healthPercentage == 0 ? "F0" : "F1";
        HealthPercentageText.text = healthPercentage.ToString(decimals) + '%';

        float barWidth = GetComponent<RectTransform>().rect.width;
        // float targetPosX = (1 - healthPercentage / 100) * -1 * barWidth + barWidth * 0.01f;
        // Vector2 targetPosition = new(targetPosX, HealthMeterImage.anchoredPosition.y);

        float targetPosX = (healthPercentage / 100) * barWidth;
        Vector2 targetPosition = new(targetPosX, HealthMeterImage.anchoredPosition.y);

        // Start smooth movement
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // Stop any ongoing movement
        }
        moveCoroutine = StartCoroutine(SmoothMove(HealthMeterImage, targetPosition, animationDuration));
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 startPosition = rect.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Normalize time (0 to 1)
            rect.anchoredPosition = Vector2.Lerp(startPosition, target, t);
            yield return null; // Wait for the next frame
        }

        rect.anchoredPosition = target; // Ensure it reaches the target position
    }
}
