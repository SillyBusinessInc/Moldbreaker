using UnityEngine;
using System.Collections;

public class SpikeField : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Damage dealt by the cone
    [SerializeField] private float cooldown = 1f; // Avoid damage loop
    [SerializeField] private float knockBackStrength = 3f; // Horizontal knockBack speed
    [Tooltip("The percentage of the knockback force that will be applied upwards, regardless of the direction that you hit the spikes")]
    [SerializeField, Range(0f,1f)] private float KnockBackUpPercentage = 0.75f;
 
    // It is static, so because of that, the cooldown is shared between all the spike cones
    private static bool playerHit = false;

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") ) return;
        if (!this.CanPlayerHit()) return;
        
        var player = GlobalReference.GetReference<PlayerReference>().Player;
        player.lastDamageCause = Player.DamageCause.HAZARD;
        var knockBackDirection = Vector3.up * this.KnockBackUpPercentage +
                                 collision.GetContact(0).normal * (1f - this.KnockBackUpPercentage);
        player.OnHit(this.damage, knockBackDirection * knockBackStrength);
    }
    
    private bool CanPlayerHit()
    {
        if (playerHit) return false;

        playerHit = true;
        this.StartCoroutine(this.ResetPlayerHit());
        return true;
    }

    // Coroutine to re-enable the spike cone after a delay
    private IEnumerator ResetPlayerHit()
    {
        yield return new WaitForSeconds(cooldown);

        playerHit = false;
    }
}
