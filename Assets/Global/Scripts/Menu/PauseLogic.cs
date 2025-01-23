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
    [SerializeField] private Image fadeImage;

    void Start()
    {
        handler.EnableInput("UI");
        Menu.SetActive(false);
        controlImage.SetActive(false);
        bgImage.SetActive(false);
        isPaused = false;
    }

    public void ContinueGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);

        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf);
        controlImage.SetActive(!controlImage.activeSelf);
        bgImage.SetActive(!bgImage.activeSelf);
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
        Time.timeScale = 1f;
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
        Time.timeScale = 1f;

        var currentScene = GetCurrentSceneName();
        if (currentScene is "PARKOUR_1" or "PARKOUR_2" or "PARKOUR_3")
            UILogic.FadeToScene("Loading", fadeImage, this);
        else 
            SceneManager.LoadScene("Menu");
    }
    
    public string GetCurrentSceneName()
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "BaseScene" && scene.name != "DontDestroyOnLoad")
                return scene.name;
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
            Time.timeScale = isPaused ? 0f : 1f;
            GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
            controlImage.SetActive(!controlImage.activeSelf);
            bgImage.SetActive(!bgImage.activeSelf);
            Image controlImage1 = controlImage.GetComponent<Image>();
            controlImage1.preserveAspect = true;
            
            var inputDevice = GetInputType();
            if ( inputDevice == "xbox") controlImage1.sprite = xboxImage;
            else if ( inputDevice == "playstation") controlImage1.sprite = playStationImage;
            else if ( inputDevice == "keyboard") controlImage1.sprite = keyboardImage;
        }

        if (isPaused)
        {
            UILogic.ShowCursor();
            return;
        }
        
        if (YoP.activeSelf)
        {
            UILogic.ShowCursor();
        }
        else
        { 
            UILogic.HideCursor(); 
            GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        }
    }
    
    string GetInputType()
    {
        var player = GlobalReference.GetReference<PlayerReference>().Player;
        var deviceLayout = player.GetComponent<PlayerInput>().currentControlScheme;
        if (Gamepad.current == null) return "keyboard";
        if (deviceLayout == "keyboard") return "keyboard";
        if (deviceLayout != "Gamepad") return "keyboard";
        
        var gamepad = Gamepad.current;
        return gamepad switch
        {
            DualShockGamepad => "playstation",
            XInputController => "xbox",
            _ => "keyboard"
        };
    }
}
