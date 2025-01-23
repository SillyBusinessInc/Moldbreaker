// using UnityEngine;
// using UnityEngine.UI;

// [RequireComponent(typeof(Image))]
// [ExecuteInEditMode]
// public class FullRectScaler : MonoBehaviour
// {
//     private Image image;
//     public float scale = 1.0f;
//     void Start()
//     {
//         image = GetComponent<Image>();
//     }

//     void Update()
//     {
//         float x = (Screen.width - 3840.0f) / -1920.0f;
//         float size = 1.555f * x + 1.444f;
//         image.pixelsPerUnitMultiplier = size * scale;
//     }
// }
