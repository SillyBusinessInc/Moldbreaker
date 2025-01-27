using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private Confirmation confirmation;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TMP_Text versionText;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        UILogic.SetCursor(true);
        continueButton.interactable = ContinueButtonActive();
        versionText.text = "v" + Application.version;
    }

    private bool ContinueButtonActive()
    {
        var directoryPath = Application.persistentDataPath;
        return Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath).Length > 0;
    }
    
    public void Continue()
    {
        UILogic.FadeToScene("Loading", fadeImage, this);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }

    public void NewGame()
    {
        if (ContinueButtonActive())
        {
            confirmation.RequestConfirmation(
                "Are you sure?", 
                "All saved progress will be deleted permanently", 
                () => ResetAllLevels(),
                EventSystem.current.currentSelectedGameObject);
        }
        else
        {
            Continue();
        }
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }

    public void ResetAllLevels()
    {
        var directoryPath = Application.persistentDataPath;
        if (Directory.Exists(directoryPath))
        {
            foreach (var file in Directory.GetFiles(directoryPath))
                File.Delete(file);
        }
        Continue();
    }

    public void OnAchievements()
    {
        UILogic.FadeToScene("Achievements", fadeImage, this);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }
    public void OnCredits()
    {
        UILogic.FadeToScene("Credits", fadeImage, this);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }
    public void OnSettings()
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void OnQuit()
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        confirmation.RequestConfirmation(
            "Are you sure?", 
            "Unsaved progress will be lost if you quit now", 
            () => Application.Quit(), 
            EventSystem.current.currentSelectedGameObject);
    }
}
