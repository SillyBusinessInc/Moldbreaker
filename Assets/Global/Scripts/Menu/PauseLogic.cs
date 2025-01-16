using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Upgrades;
    public GameObject YoP;
    private bool isPaused;
    [SerializeField] private Button continueButton;
    [SerializeField] private UIInputHandler handler;


    void Start()
    {
        Menu.SetActive(false);
        Upgrades.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    bool IsLoadingSceneLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Loading")
            {
                return false;
            }
        }
        return true;
    }

    public void ContinueGame()
    {
        isPaused = false;

        AudioManager.Instance.PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf);

        // Upgrades.SetActive(!Upgrades.activeSelf); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        isPaused = false;
        AudioManager.Instance.PlaySFX("Button");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf);
        // Upgrades.SetActive(!Upgrades.activeSelf); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        isPaused = false;
        AudioManager.Instance.PlaySFX("Button");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        }

        if (isPaused == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // handler.EnableInput("UI");
        }
        else
        {
            if (YoP.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            // handler.DisableInput("UI");
        }
    }
}
