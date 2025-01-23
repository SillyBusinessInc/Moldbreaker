using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;

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
        UILogic.ShowCursor();
        continueButton.interactable = continueButtonActive();
        versionText.text = "v" + Application.version;
    }

    public bool continueButtonActive()
    {
        string directoryPath = Application.persistentDataPath;
        if (Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath).Length > 0)
        {
            return true;
        }
        return false;
    }
    public void Continue()
    {
        UILogic.FadeToScene("Loading", fadeImage, this);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }

    public void NewGame()
    {
        if (continueButtonActive())
        {
            confirmation.RequestConfirmation("Are you sure?", "All saved progress will be deleted permanently", () => ResetAllLevels(), EventSystem.current.currentSelectedGameObject);
        }
        else
        {
            Continue();
        }
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }

    public void ResetAllLevels()
    {
        string directoryPath = Application.persistentDataPath;
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
        UILogic.FadeToScene("Settings", fadeImage, this);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
    }

    public void OnQuit()
    {
        GlobalReference.GetReference<AudioManager>().PlaySFX("Button");
        confirmation.RequestConfirmation("Are you sure?", "Unsaved progress will be lost if you quit now", () => Application.Quit(), EventSystem.current.currentSelectedGameObject);
    }
}
