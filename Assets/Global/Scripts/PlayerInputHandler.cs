using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerInteraction interactor;
    private GameManagerReference gameManager;


    // Handling the Input for the player.
    public void OnMove(InputAction.CallbackContext ctx)
    {
        player.currentState.Move(ctx);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Attack(ctx);
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Crouch(ctx);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Sprint(ctx);
    }

    public void OnGlide(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Glide(ctx);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Jump(ctx);
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        player.currentState.Dodge(ctx);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (gameManager.ignoreInput) return;
        interactor.Interact(ctx);
    }

    void Start()
    {
        gameManager = GlobalReference.GetReference<GameManagerReference>();
    }

}
