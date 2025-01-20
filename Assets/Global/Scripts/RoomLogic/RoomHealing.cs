using UnityEngine;

public class RoomHealing : MonoBehaviour
{
    private Player player;
    [SerializeField] private bool healOnEnter = false;
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GlobalReference.GetReference<PlayerReference>().Player;
        if (healOnEnter) ApplyHealing();
    }

    void OnEnable()
    {
        ApplyHealing();
    }

    void ApplyHealing()
    {
        if (!player) return;
        player.Heal(player.playerStatistic.MaxHealth.GetValue() * healPercentage);
    }
}
