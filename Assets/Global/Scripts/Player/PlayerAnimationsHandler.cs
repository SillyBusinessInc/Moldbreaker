using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    public Animator animator;

    // Method to set a boolean property
    public void SetBool(string propertyName, bool value)
    {
        animator.SetBool(propertyName, value);
    }

    // Method to set a float property
    public void SetFloat(string propertyName, float value)
    {
        animator.SetFloat(propertyName, value);
    }

    // Method to set an integer property
    public void SetInt(string propertyName, int value)
    {
        animator.SetInteger(propertyName, value);
    }

    public void ResetStates()
    {
        SetBool("IsRunning", false);
        SetBool("IsFallingDown", false);
        SetInt("AttackType", 0);
        SetInt("IdleSpecialType", 0);
        SetBool("IsJumpingBool", false);
    }
}
