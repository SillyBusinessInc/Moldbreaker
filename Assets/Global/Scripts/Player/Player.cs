using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce = 2f;
    public float airBornMovementFactor = 0.5f;
    public int doubleJumps = 1;
    public float glideDrag = 2f;
    public float dodgeRollSpeed = 10f;
    public float dodgeRollDuration = 1f;

    public float degreesToRotate = 50.0f;

    [Header("Stats")]
    public PlayerStatistic playerStatistic = new();

    [Header("Attack")]
    public float attackResettingTime = 2f;
    public float TailTurnSpeed = 40f;
    public int slamDamage = 10;
    public int firstTailDamage = 10;
    public int secondTailDamage = 15;
    public BoxCollider TransformTail;

    [Header("References")]
    [FormerlySerializedAs("playerRb")]
    public Rigidbody rb;
    public Transform orientation;

    [HideInInspector]
    public bool slamCanDoDamage = false;

    [HideInInspector]
    public int attackCounter;

    [HideInInspector]
    public int tailDoDamage;

    [HideInInspector]
    public bool isSlamming;

    [HideInInspector]
    public float activeAttackCooldown;

    [HideInInspector]
    public bool canDodgeRoll = true;

    [HideInInspector]
    public int currentJumps = 0;

    [HideInInspector]
    public float horizontalInput;

    [HideInInspector]
    public float verticalInput;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public bool tailCanDoDamage = false;

    [HideInInspector]
    public PlayerStates states;
    public StateBase currentState;

    [HideInInspector]
    public Vector2 movementInput;
    public Healthbar healthBar;

    [Header("Debugging")]
    [SerializeField]
    private string currentStateName = "none";

    // private PlayerInputActions inputActions;


    void Start()
    {
        states = new PlayerStates(this);
        SetState(states.Idle);
        // health and maxHealth should be the same value at the start of game
        playerStatistic.Health = playerStatistic.maxHealth.GetValue();
        if (healthBar) healthBar.UpdateHealthBar(0f, playerStatistic.maxHealth.GetValue(), playerStatistic.Health);
    }

    void Update()
    {
        currentState.Update();
        RotatePlayerObj();
        activeAttackCooldown =
            currentState.GetType().Name != "AttackingState"
                ? activeAttackCooldown + Time.deltaTime
                : 0.0f;
        if (activeAttackCooldown >= this.attackResettingTime)
        {
            attackCounter = 0;
            activeAttackCooldown = 0.0f;
        }
        if (isGrounded)
        {
            canDodgeRoll = true;
        }
    }

    void FixedUpdate() => currentState.FixedUpdate();

    public void OnCollisionEnter(Collision collision)
    {
        if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < degreesToRotate)
        {
            isGrounded = true;
        }
        if (isSlamming)
        {
            isSlamming = false;
            SetState(states.Idle);
        }
        currentState.OnCollision(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void SetState(StateBase newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
        currentStateName = newState.GetType().Name;
    }

    public Vector3 GetDirection()
    {
        Vector3 moveDirection =
            orientation.forward * movementInput.y + orientation.right * movementInput.x;

        return moveDirection.normalized;
    }

    private void RotatePlayerObj()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            var direction = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up).normalized;
            if (direction != Vector3.zero)
                rb.MoveRotation(Quaternion.LookRotation(direction));
        }
    }

    // If we go the event route this should change right?
    public void OnHit(float damage)
    {
        playerStatistic.Health -= damage;
        if (healthBar != null) healthBar.UpdateHealthBar(0f, playerStatistic.maxHealth.GetValue(), playerStatistic.Health);
        if (playerStatistic.Health <= 0) OnDeath();
    }

    public void Heal(float reward)
    {
        playerStatistic.Health += reward;
        healthBar.UpdateHealthBar(0f, playerStatistic.maxHealth.GetValue(), playerStatistic.Health);
    }

    public void MultiplyMaxHealth(float reward)
    {
        playerStatistic.maxHealth.AddMultiplier("reward", reward, true);
        healthBar.UpdateHealthBar(0f, playerStatistic.maxHealth.GetValue(), playerStatistic.Health);
    }

    public void IncreaseMaxHealth(float reward)
    {
        playerStatistic.maxHealth.AddModifier("reward", reward);
        healthBar.UpdateHealthBar(0f, playerStatistic.maxHealth.GetValue(), playerStatistic.Health);
    }

    // If we go the event route this should change right?
    private void OnDeath()
    {
        Debug.Log("Player died", this);
    }
}
