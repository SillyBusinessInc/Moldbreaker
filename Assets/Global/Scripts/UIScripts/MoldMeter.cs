using UnityEngine;
using TMPro;
using System.Collections;

public class MoldMeter : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f; // Duration for smooth movement
    [SerializeField] private TMP_Text HealthPercentageText;
    [SerializeField] private RectTransform HealthMeterImage;
    [SerializeField] private GameObject mold;
    [SerializeField] private GameObject moleMeter;
    
    private Coroutine moveCoroutine;
    private int cashedHealthPercentage = -1;

    private void Awake()
    {
        mold.SetActive(false);
        GlobalReference.SubscribeTo(Events.HEALTH_CHANGED, UpdateMeter);
    }

    void Start() {
        var currentScale = HealthMeterImage.localScale;
        HealthMeterImage.localScale = new(-currentScale.x, currentScale.y, currentScale.z);
    }

    public void UpdateMeter()
    {
        var player = GlobalReference.GetReference<PlayerReference>().Player;
        var maxHealth = player.playerStatistic.MaxHealth.GetValue();
        var newPercentage = (int)(player.playerStatistic.Health / maxHealth * 100);
        if (cashedHealthPercentage == newPercentage) return;
        
        cashedHealthPercentage = newPercentage;

        // If the percentage is 100% (or more for that matter),
        // it still show a little artifact of the mold which we dont want. and so that's why we turn it off
        mold.SetActive(newPercentage < 100);
        
        HealthPercentageText.text =  $"{newPercentage}%";
        var barWidth = GetComponent<RectTransform>().rect.width;
        var targetPosX = newPercentage / 100f * barWidth;
        var targetPosition = new Vector2(targetPosX, HealthMeterImage.anchoredPosition.y);

        // start smooth movement
        if (moveCoroutine != null) StopCoroutine(moveCoroutine); // stop any ongoing movement
        moveCoroutine = StartCoroutine(SmoothMove(HealthMeterImage, targetPosition, animationDuration));
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 target, float duration)
    {
        var startPosition = rect.anchoredPosition;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / duration);
            rect.anchoredPosition = Vector2.Lerp(startPosition, target, t);
            yield return null;
        }

        rect.anchoredPosition = target;
    }
}
