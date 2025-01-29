using UnityEngine;
using UnityEngine.UI;

public class DotIndicator : MonoBehaviour
{
    public Image backgroundImage;
    public Image fillImage;

    public DotIndicator(Image background, Image fill)
    {
        backgroundImage = background;
        fillImage = fill;
    }
}
