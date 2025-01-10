using System.Collections;
using UnityEngine;

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
    }
}
