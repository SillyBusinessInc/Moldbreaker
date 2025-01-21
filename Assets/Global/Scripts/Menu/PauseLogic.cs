using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    public GameObject Menu;
    public GameObject YoP;
    private bool isPaused;
    [SerializeField] private Button continueButton;
    [SerializeField] private UIInputHandler handler;

    [SerializeField] private Image fadeImage;

    void Start()
    {
        handler.EnableInput("UI");
        Menu.SetActive(false);
        isPaused = false;
    }

    public void ContinueGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);

        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf);

        // Upgrades.SetActive(!Upgrades.activeSelf); 
        UILogic.HideCursor();
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        UILogic.ShowCursor();
        Menu.SetActive(!Menu.activeSelf);
        // Upgrades.SetActive(!Upgrades.activeSelf); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        UILogic.ShowCursor();
        Menu.SetActive(!Menu.activeSelf);
        // Upgrades.SetActive(!Upgrades.activeSelf);
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Menu");

        if (GetCurrentSceneName() == "PARKOUR_1" || GetCurrentSceneName() == "PARKOUR_2" || GetCurrentSceneName() == "PARKOUR_3")
        {
            UILogic.FadeToScene("Loading", fadeImage, this);
        }
        else if (GetCurrentSceneName() == "ENTRANCE_1")
        {
            SceneManager.LoadScene("Menu");

        }
    }
    public string GetCurrentSceneName()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "BaseScene" && scene.name != "DontDestroyOnLoad")
            {
                return scene.name;
            }
        }
        return SceneManager.GetActiveScene().name;
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!YoP.activeSelf)
        {
            isPaused = !isPaused;
            Menu.SetActive(!Menu.activeSelf);
            UILogic.SelectButton(continueButton);
            // Upgrades.SetActive(!Upgrades.activeSelf); 
            Time.timeScale = isPaused ? 0f : 1f;
            GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
        }

        if (isPaused == true)
        {
            UILogic.ShowCursor();
            // handler.EnableInput("UI");
        }
        else
        {
            if (YoP.activeSelf)
            {
                UILogic.ShowCursor();
            }
            else
            {
                UILogic.HideCursor();
                GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
                // handler.DisableInput("UI");
            }
        }
    }
}
