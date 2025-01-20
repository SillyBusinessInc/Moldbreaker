using System.Collections;
using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private CrossfadeController crossfadeController;
    [SerializeField] private DeathMenu deathMenu;

    private void Start()
    {
        StartCoroutine(GameOver());
        UILogic.ShowCursor();
    }

    private IEnumerator GameOver()
    {
        yield return StartCoroutine(crossfadeController.Crossfade_End());
        deathMenu.isDead = true;
    }
}
