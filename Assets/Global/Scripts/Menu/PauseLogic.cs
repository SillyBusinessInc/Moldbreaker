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
    [SerializeField] private Sprite keyboardControlImage;
    [SerializeField] private Sprite ControllerImage;
    [SerializeField] private GameObject controlImage;

    void Start()
    {
        handler.EnableInput("UI");
        Menu.SetActive(false);
        controlImage.SetActive(false);
        isPaused = false;
    }

    public void ContinueGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);

        AudioManager.Instance.PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf);
        controlImage.SetActive(!controlImage.activeSelf);

        // Upgrades.SetActive(!Upgrades.activeSelf); 
        UILogic.HideCursor();
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        AudioManager.Instance.PlaySFX("Button");
        UILogic.ShowCursor();
        Menu.SetActive(!Menu.activeSelf);
        controlImage.SetActive(!controlImage.activeSelf);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        AudioManager.Instance.PlaySFX("Button");
        UILogic.ShowCursor();
        Menu.SetActive(!Menu.activeSelf);
        controlImage.SetActive(!controlImage.activeSelf);
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
            Time.timeScale = isPaused ? 0f : 1f;
            GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
            controlImage.SetActive(!controlImage.activeSelf);
            Image controlImage1 = controlImage.GetComponent<Image>();
            if (!IsControllerInput()) {
                Debug.Log("keyboardController Image");
                controlImage1.sprite = keyboardControlImage;
            } else {
                Debug.Log("Controller Image");
                controlImage1.sprite = ControllerImage;
            }
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
    bool IsControllerInput()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed) return false;
        return true;
    }
}