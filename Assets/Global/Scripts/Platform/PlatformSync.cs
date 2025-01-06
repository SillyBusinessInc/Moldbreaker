using Unity.VisualScripting;
using UnityEngine;

public class PlatformSync : MonoBehaviour
{
    private Player player;

    private void OnCollisionEnter(Collision other)
    {
        other.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision other)
    {
        player = GlobalReference.GetReference<PlayerReference>().Player;
        other.transform.SetParent(player.transform);
    }
}
