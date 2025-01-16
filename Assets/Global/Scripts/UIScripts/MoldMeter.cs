using UnityEngine;
using TMPro;
using System.Collections;

public class MoldMeter : MonoBehaviour
{
    private Player player;
    [SerializeField] private TMP_Text MoldPercentageText;
    [SerializeField] private float moldPercentage;
    [SerializeField] private RectTransform MoldMeterImage;
    private Coroutine moveCoroutine;
    [SerializeField] private float animationDuration = 0.5f; // Duration for smooth movement
    private float savedMoldPercentage = -1;

    void Start()
    {
        player = GlobalReference.GetReference<PlayerReference>().Player;

        GlobalReference.SubscribeTo(Events.MOLDMETER_CHANGED, UpdateMoldMeter);
    }

    void Update() => UpdateMoldMeter();

    public void UpdateMoldMeter()
    {
     //    moldPercentage = player.playerStatistic.Moldmeter;
     //    // if (moldPercentage < 100) moldPercentage += 0.01f;
     //    if (savedMoldPercentage == moldPercentage) return;
     //    savedMoldPercentage = moldPercentage;
     //    
     //    string decimals = moldPercentage >= 100 || moldPercentage == 0 ? "F0" : "F1";
     //    MoldPercentageText.text = moldPercentage.ToString(decimals) + '%';
// 
     //    float barWidth = GetComponent<RectTransform>().rect.width;
     //    float targetPosX = (1 - moldPercentage / 100) * -1 * barWidth + barWidth * 0.01f;
     //    Vector2 targetPosition = new(targetPosX, MoldMeterImage.anchoredPosition.y);
// 
     //    // Start smooth movement
     //    if (moveCoroutine != null)
     //    {
     //        StopCoroutine(moveCoroutine); // Stop any ongoing movement
     //    }
     //    moveCoroutine = StartCoroutine(SmoothMove(MoldMeterImage, targetPosition, animationDuration));
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
