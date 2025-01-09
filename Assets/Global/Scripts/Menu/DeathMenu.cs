using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public GameObject Menu;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public PreviousLevel previousLevel;
    [SerializeField] private Button retry;
    [SerializeField] private Image fadeImage;


    void Start()
    {
        previousLevel = GlobalReference.GetReference<PreviousLevel>();
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isDead)
        {
            if (!Menu.activeSelf) Menu.SetActive(true);
            if (previousLevel) retry.interactable = previousLevel.prevLevel.IsUnityNull() ? false : true;
            else retry.interactable = false;
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
        Debug.Log($"{previousLevel.prevLevel} | {SceneManager.GetSceneByBuildIndex(previousLevel.prevLevel)}");
        if (!previousLevel.prevLevel.IsUnityNull()) UILogic.FadeToScene("Loading", fadeImage, this);
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
