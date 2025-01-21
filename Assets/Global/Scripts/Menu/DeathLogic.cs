using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private DeathMenu deathMenu;

    private void Start()
    {
        deathMenu.isDead = true;
        UILogic.ShowCursor();
    }
}
