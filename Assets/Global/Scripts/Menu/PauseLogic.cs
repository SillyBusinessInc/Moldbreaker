using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Upgrades;
    public GameObject YoP;
    private bool isPaused;


    void Start() {
        Menu.SetActive(false);
        Upgrades.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && IsLoadingSceneLoaded()) { 
            if (!YoP.activeSelf) {
                isPaused = !isPaused;
                Menu.SetActive(!Menu.activeSelf); 
                // Upgrades.SetActive(!Upgrades.activeSelf); 
                Time.timeScale = isPaused ? 0f : 1f;
            }
        }

        if (isPaused == true) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    bool IsLoadingSceneLoaded() { 
        for (int i = 0; i < SceneManager.sceneCount; i++) { 
            Scene scene = SceneManager.GetSceneAt(i); 
            if (scene.name == "Loading") { 
                return false; 
            }
        }
        return true;
    }

    public void ContinueGame() {
        isPaused = false;
        AudioManager.Instance.PlaySFX("Button");
        Menu.SetActive(!Menu.activeSelf); 
        // Upgrades.SetActive(!Upgrades.activeSelf); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void Settings() {
        isPaused = false;
        AudioManager.Instance.PlaySFX("Button");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf); 
        // Upgrades.SetActive(!Upgrades.activeSelf); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame() {
        isPaused = false;
        AudioManager.Instance.PlaySFX("Button");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf); 
        // Upgrades.SetActive(!Upgrades.activeSelf);
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu");
    }
}
