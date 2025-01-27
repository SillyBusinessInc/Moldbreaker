using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private static GameObject defaultSelectedButton;

    void Start()
    {
        handler.EnableInput("UI");
        Menu.SetActive(false);
        controlImage.SetActive(false);
        bgImage.SetActive(false);
        isPaused = false;
        defaultSelectedButton = transform.GetChild(1).GetChild(1).gameObject;
        
        GlobalReference.SubscribeTo(Events.DEVICE_CHANGED, OnDeviceChanged);
    }
    
    private void OnDeviceChanged()
    {
        if (!isPaused) return;
        
        var inputDevice = UILogic.GetInputType();
        UILogic.SetCursor(inputDevice == "keyboard");
        
        var controlImage1 = controlImage.GetComponent<Image>();
        controlImage1.preserveAspect = true;
        if ( inputDevice == "xbox") controlImage1.sprite = xboxImage;
        else if ( inputDevice == "playstation") controlImage1.sprite = playStationImage;
        else if ( inputDevice == "keyboard") controlImage1.sprite = keyboardImage;
    }

    private void SetPauseState(bool value)
    {
        Menu.SetActive(value);
        controlImage.SetActive(value);
        bgImage.SetActive(value);
        isPaused = value;
        
        UILogic.SetCursor(value && UILogic.GetInputType() == "keyboard");
        Time.timeScale = value ? 0f : 1f;
        GlobalReference.AttemptInvoke(value ? Events.INPUT_IGNORE : Events.INPUT_ACKNOWLEDGE);
        GlobalReference.AttemptInvoke(value ? Events.SPEEDRUN_MODE_INACTIVE : Events.SPEEDRUN_MODE_ACTIVE);
    }

    public static void ForceSelectDefault() {
        if (defaultSelectedButton != null) EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }
    
    public void ContinueGame()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_ACTIVE);

        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        SetPauseState(false);
    }

    public void OnSettings()
    {
        isPaused = false;
        GlobalReference.AttemptInvoke(Events.INPUT_ACKNOWLEDGE);
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_INACTIVE);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
     
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        SetPauseState(false);
        GlobalReference.AttemptInvoke(Events.SPEEDRUN_MODE_INACTIVE);
        GlobalReference.GetReference<PlayerReference>().Player.speedrunMode.ResetTimers();
        
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
        if (IsSettingsSceneLoaded()) return;
        if (YoP.activeSelf) return; // you cant pause the game if you are in the YoP window
        
        if (GetCurrentSceneName() is "PARKOUR_1" or "PARKOUR_2" or "PARKOUR_3")
            quitButtonText.text = "Back to hub";
        else 
            quitButtonText.text = "Quit to menu";
        
        SetPauseState(!isPaused);
        
        UILogic.SelectButton(continueButton);
        OnDeviceChanged();
    }

    private bool IsSettingsSceneLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Settings") return true;
        }
        return false;
    }
}
