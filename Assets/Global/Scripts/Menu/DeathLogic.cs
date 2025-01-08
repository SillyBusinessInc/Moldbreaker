using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private CrossfadeController crossfadeController;
    [SerializeField] private DeathMenu deathMenu;

    private void Start()
    {
        StartCoroutine(GameOver());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator GameOver()
    {
        yield return StartCoroutine(crossfadeController.Crossfade_End());
        deathMenu.isDead = true;
        // yield return StartCoroutine(WaitForFewSecond());
        // yield return StartCoroutine(crossfadeController.Crossfade_Start());

        // SceneManager.LoadScene("Menu");
    }

    private IEnumerator WaitForFewSecond()
    {
        yield return new WaitForSeconds(3f);
    }
}
