using UnityEngine;
using TMPro;
using System.Linq;

public class CaloriesUI : MonoBehaviour
{
    private TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
    private PlayerStatistic playerStats; // Reference to the player's statistics
    [SerializeField] private float fadeDuration = 1f;
    private float fadeProgress = 0f;
    [SerializeField]private CanvasGroup canvasGroup;
    [SerializeField]private GameObject pauseScreen;

    void Start()
    {
        fadeProgress = fadeDuration * 4f;
        
        // Get the player's statistics from your player reference
        playerStats = GlobalReference.GetReference<PlayerReference>().Player.playerStatistic;

        // Get the TextMeshProUGUI component attached to this GameObject
        text = GetComponent<TextMeshProUGUI>();

        if (text == null)
        {
            Debug.LogWarning("TextMeshProUGUI component not found!");
        }
    }

    void Update()
    {
        // Reset fade when a new coin is collected
        if (playerStats.CollectedCalorie)
        {
            fadeProgress = 0f;
            playerStats.CollectedCalorie = false;
        }

        // Fade in (first segment), hold at 1 (second segment), then fade out (third segment)
        if (!pauseScreen.activeSelf)
        {
            fadeProgress += Time.deltaTime;

            if (fadeProgress < fadeDuration)
            {
                float t = fadeProgress / fadeDuration; // 0 to 1
                canvasGroup.alpha = Mathf.Lerp(0f, 2f, t*4);
            }
            else if (fadeProgress < fadeDuration * 2f)
            {
                canvasGroup.alpha = 1f; // hold at 1
            }
            else if (fadeProgress < fadeDuration * 3f)
            {
                float t = (fadeProgress - fadeDuration * 2f) / fadeDuration; // 0 to 1
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t*2);
            }
            else
            {
                canvasGroup.alpha = 0f;
            }
        }
        else
        {   
            if (text.text == "0/0") canvasGroup.alpha = 0f;
            else canvasGroup.alpha = pauseScreen.activeSelf ? 1f : 0f;
        }
            

        // Maintain existing logic
        if (playerStats.CaloriesCount == 0)
        {
            playerStats.CaloriesCount = GameObject.FindGameObjectsWithTag("Calories").Count();
        }

        if (text != null)
        {
            text.text = playerStats.caloriesCountExtra + playerStats.Calories.Count() + "/" + playerStats.CaloriesCount;
        }
    }
}
