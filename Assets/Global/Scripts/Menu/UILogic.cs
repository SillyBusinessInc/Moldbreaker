using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class UILogic
{
    private static bool listening = true;

    public static void FadeToScene(string sceneName, Image fadeImage, MonoBehaviour target)
    {
        if (listening) listening = false;
        else return;
        SetAlpha(0, fadeImage);
        fadeImage.gameObject.SetActive(true);
        target.StartCoroutine(Fade(sceneName, fadeImage));
    }

    private static IEnumerator Fade(string sceneName, Image fadeImage)
    {
        for (float i = 0; i <= 1.1f; i += Time.deltaTime * 2)
        {
            SetAlpha(i, fadeImage);
            yield return null;
        }
        PostFade(sceneName);
    }

    private static void PostFade(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        listening = true;
    }

    private static void SetAlpha(float alpha, Image fadeImage)
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
    }

    public static void FlipInteractability(params CanvasGroup[] canvasGroups)
    {
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            canvasGroup.interactable = !canvasGroup.interactable;
        }
    }

    public static void SelectButton(Button btn)
    {
        if (btn) btn.Select();
    }
}
