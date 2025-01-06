using UnityEngine;
using UnityEngine.InputSystem;
public class UIInputHandler : MonoBehaviour
{
    [SerializeField] private UpgradeOptions upgradeOptions;
    public void OnConfirm(InputAction.CallbackContext ctx) {
        upgradeOptions.Confirm(ctx);
    }
}