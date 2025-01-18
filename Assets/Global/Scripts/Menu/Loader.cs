using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    private Image loadingBar;
    private TMP_Text message;
    private float loadingProgress;
    private float targetLoadingProgress;
    private Phase nextPhase;
    private int currentPhase;
    private int totalPhases;

    void Start()
    {
        loadingBar = transform.GetChild(0).GetComponent<Image>();
        message = transform.GetChild(1).GetComponent<TMP_Text>();
        loadingBar.fillAmount = loadingProgress;
        nextPhase = Phase.LOADBASE;

        loadingProgress = 0;
        currentPhase = 0;
        totalPhases = 4;

        Time.timeScale = 0;

        UpdateProgress();

        // while (nextPhase != Phase.NONE) NextPhase();
        StartCoroutine(WaitForPhase());
    }

    void Update()
    {
        if (currentPhase >= totalPhases - 1) targetLoadingProgress = 100;
        loadingProgress += (targetLoadingProgress - loadingProgress) * 0.99f * Time.unscaledDeltaTime; ;

        loadingBar.fillAmount = Mathf.Clamp(loadingProgress / 100f, 0f, 1f);
    }

    private void UpdateProgress()
    {
        currentPhase++;

        targetLoadingProgress = 100f / (totalPhases - 1) * currentPhase + Random.Range(-10f, 10f);
    }

    private void NextPhase()
    {
        UpdateProgress();
        switch (nextPhase)
        {
            case Phase.LOADBASE:
                nextPhase = LoadBaseScene();
                break;

            case Phase.LOADROOM:
                nextPhase = LoadRoom(PreviousLevel.Instance?.prevLevel);
                break;

            case Phase.INITBASE:
                nextPhase = InitializeBaseScene();
                break;

            case Phase.COMPLETE:
                nextPhase = CompleteLoading();
                break;
            default:
                break;
        }
        StartCoroutine(WaitForPhase());
    }

    public IEnumerator WaitForPhase()
    {
        yield return new WaitForSecondsRealtime(3);
        NextPhase();
    }

    private enum Phase
    {
        NONE,
        LOADBASE,
        LOADROOM,
        INITBASE,
        COMPLETE
    }

    // loading logic
    private Phase LoadBaseScene()
    {
        message.text = "Rising the bread world...";
        SceneManager.LoadScene("BaseScene", LoadSceneMode.Additive);
        return Phase.LOADROOM;
    }

    private Phase LoadRoom(int? level)
    {
        message.text = "Baking the bread world...";

        if (PreviousLevel.Instance != null && level.HasValue && level.Value > 0)
        {
            GameManagerReference gm = GlobalReference.GetReference<GameManagerReference>();
            gm.activeRoom = gm.GetRoom(level ?? 0);
            Debug.LogWarning($"setting room to {level ?? 0}");
            SceneManager.LoadScene(level.Value, LoadSceneMode.Additive);
            // reset prevLevel after initiating load of previous level
            PreviousLevel.Instance.ResetPreviousLevel();

        }
        else
        {
            SceneManager.LoadScene("ENTRANCE_1", LoadSceneMode.Additive);
        }

        return Phase.INITBASE;
    }

    private Phase InitializeBaseScene()
    {
        message.text = "Eating the bread world...";
        GlobalReference.GetReference<GameManagerReference>().Initialize();
        GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
        return Phase.COMPLETE;
    }

    private Phase CompleteLoading()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Loading");
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        return Phase.NONE;
    }
}
