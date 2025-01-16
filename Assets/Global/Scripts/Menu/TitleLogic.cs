using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    
    public Texture2D cursorTex;    
    void Awake()
    {
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);

        // if (SteamManager.Initialized) SteamUserStats.ResetAllStats(true);
    }

    void Start() {
        fadeImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKey){
            AudioManager.Instance.PlaySFX("Button");
            UILogic.FadeToScene("Menu", fadeImage, this);
        } 
    }
}
