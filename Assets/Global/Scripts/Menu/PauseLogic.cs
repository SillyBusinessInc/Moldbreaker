using TMPro;
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
    [SerializeField] private TMP_Text quitButtonText;
    void Start()
    {
        handler.EnableInput("UI");
        SetPauseState(false);
    }

    private void SetPauseState(bool value)
    {
        Menu.SetActive(value);
        controlImage.SetActive(value);
        bgImage.SetActive(value);
        isPaused = value;
        Time.timeScale = value ? 0f : 1f;
        UILogic.SetCursor(value);
    }
    
    public void ContinueGame()
    {
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        SetPauseState(false);
    }

    public void Settings()
    {
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        SetPauseState(false);
        
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");

        SetPauseState(false);
        
        if (GetCurrentSceneName() is "PARKOUR_1" or "PARKOUR_2" or "PARKOUR_3")
            UILogic.FadeToScene("Loading", fadeImage, this);
        else 
            SceneManager.LoadScene("Menu");
    }
    
    private string GetCurrentSceneName()
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
        
        if (GetCurrentSceneName() is "PARKOUR_1" or "PARKOUR_2" or "PARKOUR_3")
            quitButtonText.text = "Back to hub";
        else 
            quitButtonText.text = "Quit to menu";
        
        if (!YoP.activeSelf)
        {
            SetPauseState(!isPaused);
          
            UILogic.SelectButton(continueButton);
            GlobalReference.AttemptInvoke(Events.INPUT_IGNORE);
            var controlImage1 = controlImage.GetComponent<Image>();
            controlImage1.preserveAspect = true;
            
            var inputDevice = GetInputType();
            if ( inputDevice == "xbox") controlImage1.sprite = xboxImage;
            else if ( inputDevice == "playstation") controlImage1.sprite = playStationImage;
            else if ( inputDevice == "keyboard") controlImage1.sprite = keyboardImage;
        }

        if (isPaused)
        {
            UILogic.SetCursor(true);
            return;
        }
        
        if (YoP.activeSelf)
        {
            UILogic.SetCursor(true);
        }
        else
        { 
            UILogic.SetCursor(false); 
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
