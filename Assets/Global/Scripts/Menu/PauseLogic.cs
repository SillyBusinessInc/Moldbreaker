using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    public GameObject Menu;
    public GameObject YoP;
    private bool isPaused;
    [SerializeField] private Button continueButton;
    [SerializeField] private UIInputHandler handler;
    [SerializeField] private GameObject controlImage;
    [SerializeField] private Sprite keyboardImage;
    [SerializeField] private Sprite playStationImage;
    [SerializeField] private Sprite xboxImage;
    [SerializeField] private GameObject bgImage;

    private PlayerInput playerInput;


    void Start()
    {
        handler.EnableInput("UI");
        Menu.SetActive(false);
        controlImage.SetActive(false);
        bgImage.SetActive(false);
        isPaused = false;
        playerInput = GlobalReference.GetReference<PlayerReference>().Player.GetComponent<PlayerInput>();

    }

    public void ContinueGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);

        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf);
        controlImage.SetActive(!controlImage.activeSelf);
        bgImage.SetActive(!bgImage.activeSelf);
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
        controlImage.SetActive(!controlImage.activeSelf);
        bgImage.SetActive(!bgImage.activeSelf);
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
        controlImage.SetActive(!controlImage.activeSelf);
        bgImage.SetActive(!bgImage.activeSelf);
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
            Time.timeScale = isPaused ? 0f : 1f;
            GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
            controlImage.SetActive(!controlImage.activeSelf);
            bgImage.SetActive(!bgImage.activeSelf);
            Image controlImage1 = controlImage.GetComponent<Image>();
            if (IsControllerInput() == "xbox") controlImage1.sprite = xboxImage;
            else if (IsControllerInput() == "playstation") controlImage1.sprite = playStationImage;
            else if (IsControllerInput() == "keyboard") controlImage1.sprite = keyboardImage;
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
    string IsControllerInput()
    {
        string deviceLayout = playerInput.currentControlScheme;
        if (Gamepad.current != null) {
            if (deviceLayout == "keyboard") return "keyboard";
            else if (deviceLayout == "Gamepad") {
                var gamepad = Gamepad.current;
                if (gamepad is DualShockGamepad)
                {
                    return "playstation";
                }
                else if (gamepad is XInputController)
                {
                    return "xbox";
                }
            }
        }
        return "keyboard";
    }
}
