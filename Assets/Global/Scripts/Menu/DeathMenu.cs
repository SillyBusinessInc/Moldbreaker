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
        UILogic.SetCursor(false);

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
        if (isDead && !Menu.activeSelf) Menu.SetActive(true);
        if (isDead && Cursor.lockState == CursorLockMode.Locked) UILogic.SetCursor(true);
    }

    public void RestartLevel()
    {
        // reset the current level
        Menu.SetActive(!Menu.activeSelf);
        UILogic.SetCursor(false);
        if (PreviousLevel.Instance) UILogic.FadeToScene("Loading", fadeImage, this);
    }

    public void QuitGame()
    {
        isDead = false;
        UILogic.SetCursor(true);
        Menu.SetActive(!Menu.activeSelf);
        if (PreviousLevel.Instance) PreviousLevel.Instance.ResetLevelForRetry();

        SceneManager.LoadScene("Menu");
    }
}
