using System.Collections;
using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    public Animator animator;

    public void ResetStates()
    {
        SetBool("IsRunning", false);
        SetBool("IsFallingDown", false);
        SetInt("AttackType", 0);
        SetInt("IdleSpecialType", 0);
        SetBool("IsJumpingBool", false);
    }
    
    public void SetBool(string propertyName, bool value) => animator.SetBool(propertyName, value);
    public void SetInt(string propertyName, int value) => animator.SetInteger(propertyName, value);

    // Animation events handlers
    private void StepSound()
    {
        StartCoroutine(PlayStepSound());
    }

    private IEnumerator PlayStepSound()
    {
        yield return new WaitForSeconds(Random.value * 0.1f);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Footstep");
    }
}
