using UnityEngine;
using TMPro;
using System.Collections;

public class MoldMeter : MonoBehaviour
{
    [SerializeField] private TMP_Text HealthPercentageText;
    [SerializeField] private float healthPercentage;
    [SerializeField] private RectTransform HealthMeterImage;
    [SerializeField] private float animationDuration = 0.5f; // Duration for smooth movement
    [SerializeField] private GameObject mold;
    
    private Coroutine moveCoroutine;
    private float savedHealthPercentage = -1;
    private float playerMaxHealth = -1;
    private Player player;
    
    void Awake()
    {
        GlobalReference.SubscribeTo(Events.MOLDMETER_CHANGED, this.UpdateMeter);
        mold.SetActive(false);
    }

    void Start() {
        player = GlobalReference.GetReference<PlayerReference>().Player;
        var currentScale = HealthMeterImage.localScale;
        HealthMeterImage.localScale = new(-currentScale.x, currentScale.y, currentScale.z);
    }

    void Update() => this.UpdateMeter();

    public void UpdateMeter()
    {
        if (this.playerMaxHealth == -1) this.playerMaxHealth = player.playerStatistic.Health;
        healthPercentage = player.playerStatistic.Health / this.playerMaxHealth * 100;
        if (savedHealthPercentage == healthPercentage) return;
        savedHealthPercentage = healthPercentage;

        // If the percentage is 100% (or more for that matter),
        // it still show a little artifact of the mold which we dont want. and so that's why we turn it off
        this.mold.SetActive(this.healthPercentage < 100);
        
        HealthPercentageText.text =  $"{healthPercentage}%";

        var barWidth = GetComponent<RectTransform>().rect.width;
        var targetPosX = healthPercentage / 100 * barWidth;
        var targetPosition = new Vector2(targetPosX, HealthMeterImage.anchoredPosition.y);

        // start smooth movement
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // stop any ongoing movement
        }
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
