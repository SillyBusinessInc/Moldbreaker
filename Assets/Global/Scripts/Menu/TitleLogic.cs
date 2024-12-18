using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogic : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    
    public Texture2D cursorTex;    
    void Awake()
    {
        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Start() {
        fadeImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKey) UILogic.FadeToScene("Menu", fadeImage, this);
    }
}
