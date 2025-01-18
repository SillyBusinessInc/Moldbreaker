using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class MenuLogic : MonoBehaviour
{
    [SerializeField] private Confirmation confirmation;
    [SerializeField] private Image fadeImage;

    [SerializeField] private Button continueButton;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        continueButton.interactable = continueButtonActive();

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
        AudioManager.Instance.PlaySFX("Button");
    }

    public void NewGame()
    {
        if (continueButtonActive())
        {
            confirmation.RequestConfirmation("Are you sure?", "All saved progress will be deleted permanently", () => ResetAllLevels());
        }
        else
        {
            Continue();
        }
        AudioManager.Instance.PlaySFX("Button");
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
        AudioManager.Instance.PlaySFX("Button");
    }
    public void OnCredits()
    {
        UILogic.FadeToScene("Credits", fadeImage, this);
        AudioManager.Instance.PlaySFX("Button");
    }
    public void OnSettings()
    {
        UILogic.FadeToScene("Settings", fadeImage, this);
        AudioManager.Instance.PlaySFX("Button");
    }

    public void OnQuit()
    {
        AudioManager.Instance.PlaySFX("Button");
        confirmation.RequestConfirmation("Are you sure?", "Unsaved progress will be lost if you quit now", () => Application.Quit());
    }
}
