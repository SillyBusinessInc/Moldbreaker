using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject Menu;
    [HideInInspector]
    public bool isDead = false;
    [SerializeField] private Button retry;
    [SerializeField] private Button quit;
    [SerializeField] private Image fadeImage;


    void Start()
    {
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PreviousLevel.Instance && PreviousLevel.Instance.prevLevel > 0)
        {
            retry.interactable = true;
        }
        else
        {
            retry.interactable = false;
            UILogic.SelectButton(quit);
        }
    }

    void Update()
    {
        if (isDead)
        {
            if (!Menu.activeSelf) Menu.SetActive(true);

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
        if (PreviousLevel.Instance) UILogic.FadeToScene("Loading", fadeImage, this);
    }

    public void QuitGame()
    {
        isDead = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.SetActive(!Menu.activeSelf);
        if (PreviousLevel.Instance) PreviousLevel.Instance.ResetPreviousLevel();

        SceneManager.LoadScene("Menu");
    }
}
