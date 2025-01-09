using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject Menu;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    [SerializeField] private Button retry;
    [SerializeField] private Image fadeImage;


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
            if (!Menu.activeSelf) Menu.SetActive(true);
            if (PreviousLevel.Instance != null && PreviousLevel.Instance.prevLevel > 0) retry.interactable = true;

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
        if (PreviousLevel.Instance != null) UILogic.FadeToScene("Loading", fadeImage, this);
    }

    public void QuitGame()
    {
        isDead = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf);

        SceneManager.LoadScene("Menu");
    }
}
