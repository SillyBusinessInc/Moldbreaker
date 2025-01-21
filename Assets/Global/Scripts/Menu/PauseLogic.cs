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
        // SceneManager.LoadScene("Settings");
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
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
        SceneManager.LoadScene("Menu");
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
