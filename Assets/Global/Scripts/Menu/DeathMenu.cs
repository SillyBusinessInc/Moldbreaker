using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject Menu;
    [HideInInspector]
    public bool isDead = false;


    void Start()
    {
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isDead)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void RestartLevel()
    {
        // reset the current level
        Menu.SetActive(!Menu.activeSelf);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        isDead = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
