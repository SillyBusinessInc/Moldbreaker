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
        UILogic.HideCursor();

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
    }

    public void RestartLevel()
    {
        // reset the current level
        Menu.SetActive(!Menu.activeSelf);
        UILogic.HideCursor();
        if (PreviousLevel.Instance) UILogic.FadeToScene("Loading", fadeImage, this);
    }

    public void QuitGame()
    {
        isDead = false;
        UILogic.ShowCursor();
        Menu.SetActive(!Menu.activeSelf);
        if (PreviousLevel.Instance) PreviousLevel.Instance.ResetLevelForRetry();

        SceneManager.LoadScene("Menu");
    }
}
