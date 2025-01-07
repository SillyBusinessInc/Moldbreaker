using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;

public static class ApplyKnockbackToGO
{
    public static void ApplyKnockback(GameObject target, Vector3 knockback, float duration, MonoBehaviour coroutineRunner)
    {
        if (!target)
        {
            Debug.LogWarning("Target GameObject is null. Knockback not applied.");
            return;
        }

        if (target.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = knockback;

            // check if its a player 
            if (target.CompareTag("Player"))
            {
                Player player = target.TryGetComponent<Player>(out var playerComponent) ? playerComponent : null;

                if (player != null)
                {
                    player.isKnockedBack = true;
                }
            }

            // Start stun routine to disable knockback after the specified duration
            coroutineRunner.StartCoroutine(KnockbackStunRoutine(duration, target));
        }
        else
        {
            Debug.LogWarning($"GameObject {target.name} does not have a Rigidbody component. Knockback not applied.");
        }
    }

    private static IEnumerator KnockbackStunRoutine(float duration, GameObject target)
    {
        // Wait for the stun duration
        yield return new WaitForSecondsRealtime(duration);

        if (target == null) yield break;

        if (target.CompareTag("Player"))
        {
            Player player = target.GetComponent<Player>();

            if (player != null)
            {
                player.isKnockedBack = false;
            }

        }
    }

    public static Vector3 CalculateKnockback(GameObject source, GameObject target, float knockbackForce, float leapForce = 0f)
    {
        if (!source || !target)
        {
            Debug.LogWarning("Source or target GameObject is null. Knockback not calculated.");
            return Vector3.zero;
        }

        // Calculate direction from source to target
        Vector3 direction = (target.transform.position - source.transform.position).normalized;

        // Create a vector with constant magnitude equal to knockbackForce
        Vector3 knockbackVelocity = direction * knockbackForce;

        // Add upward force for leap
        knockbackVelocity.y += leapForce;

        Debug.Log($"Knockback: {knockbackVelocity}");
        return knockbackVelocity;
    }


}