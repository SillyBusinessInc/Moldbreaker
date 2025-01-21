using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Walking Settings")]
    public float acceleration = 2;
    public float deceleration = 0.5f;
    public float currentMovementLerpSpeed = 100;
    [Tooltip("The time it takes before it starts the step sounds")]
    public float InitialStepSoundDelay = 0.5f;

    [Header("Knockback Settings")]
    public float knockbackDuration;
    public float knockbackSpeed;
    
    [Header("Jumping Settings")]
    public float maxJumpHoldTime = 0.2f;
    public float airBorneMovementFactor = 0.5f;
    public float fallMultiplier = 7;
    public float jumpVelocityFalloff = 8;
    public float coyoteTime = 0.3f;

    [Header("Other Settings")]
    public float dodgeRollSpeed = 10f;
    public float dodgeRollDuration = 1f;
    public float dodgeRollDeceleration = 1f;
    public float groundCheckAngle = 50.0f;
    
    [Tooltip("Max time before it plays a special idle animation")]
    public float maxIdleAnimTime = 20f;
    [Tooltip("Min time before it plays a special idle animation")]
    public float MinIdleAnimTime = 5f;
    
    [SerializeField] private float invulnerabilityTime = 0.5f;
    [SerializeField] private Transform cameraTarget;
    private Vector3 defaultCameraTarget = Vector3.zero;

    [Header("Stats")]
    public PlayerStatistic playerStatistic = new();
    [SerializeField] private List<UpgradeOption> upgrades;
    public Tail Tail;

    [Header("References")]
    [FormerlySerializedAs("playerRb")]
    public Rigidbody rb;
    public SkinnedMeshRenderer mr;
    public SkinnedMeshRenderer tailmr;
    public Transform orientation;
    public ParticleSystem particleSystemJump;
    public ParticleSystem particleSystemDash;
    public ParticleSystem particleSystemWalk;

    [HideInInspector] public PlayerAnimationsHandler playerAnimationsHandler;
    [HideInInspector] public bool canDodgeRoll = true;
    [HideInInspector] public int currentJumps = 0;
    [HideInInspector] public PlayerStates states;
    [HideInInspector] public StateBase currentState;
    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public List<Collider> collidersEnemy;
    [HideInInspector] public float groundCheckDistance;
    [HideInInspector] public Vector3 targetVelocity;
    [HideInInspector] public float timeLastDodge;
    [HideInInspector] public float currentWalkingPenalty;
    [HideInInspector] public float maxWalkingPenalty = 0.5f;
    [HideInInspector] public int recentHits = 0;
    [HideInInspector] public int succesfullHitCounter = 0;
    [HideInInspector] public DamageCause lastDamageCause = DamageCause.NONE;
    [HideInInspector] public bool roomInvulnerability = false;
    private float currentMoldPercentage = 0;
    public Coroutine activeCoroutine;
    
    [Header("Debugging")]
    [SerializeField] public bool isGrounded;
    [SerializeField] private string debug_currentStateName = "none"; // only for the inspector
    [HideInInspector] public Color debug_lineColor; // gizmos line that changes color based on state
    [SerializeField] private bool isKnockedBack = false;
    [HideInInspector] public bool isHoldingJump = false;
    [HideInInspector] public bool isHoldingDodge = false;
    [HideInInspector] public bool AirComboDone = false;
    [HideInInspector] public Vector3 hitDirection;
    private bool IsLanding = false;
    [SerializeField] private CrossfadeController crossfadeController;
    [HideInInspector] public bool isInvulnerable = false;

    void Awake()
    {
        playerStatistic.Generate();

        GlobalReference.SubscribeTo(Events.LEVELS_CHANGED_BY_CHEAT, AlreadyRecievedUpgrades);
    }

    void Start()
    {
        playerAnimationsHandler = GetComponent<PlayerAnimationsHandler>();
        states = new PlayerStates(this);
        SetState(states.Idle);

        collidersEnemy = new List<Collider>();

        playerStatistic.Health = playerStatistic.MaxHealth.GetValue();
        GlobalReference.AttemptInvoke(Events.HEALTH_CHANGED);
        defaultCameraTarget = cameraTarget.localPosition;
        
        AlreadyRecievedUpgrades();
    }

    private void AlreadyRecievedUpgrades()
    {
        var saveData = new RoomSave();
        saveData.LoadAll();
        var list = saveData.Get<List<int>>("finishedLevels");
        for (var i = 0; i < upgrades.Count; i++)
        {
            if (list.Contains(i + 1)) upgrades[i].interactionActions.ForEach(action => action.InvokeAction());
        }
    }
    
    void Update()
    {
        GroundCheck();
        CheckLandingAnimation();
        currentState.Update();
        ApproachTargetVelocity();
        RotatePlayerObj();
        if (isGrounded) AirComboDone = false;
        if (isGrounded) canDodgeRoll = true;
        UpdateVisualState();
    }

    // Setting the height to null will reset the height to default
    public void SetCameraHeight(float? height)
    {
        if (height == null) cameraTarget.localPosition = defaultCameraTarget;
        else cameraTarget.localPosition = new Vector3(0, (float)height, 0);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(rb.position, rb.position + targetVelocity, debug_lineColor, 0, true);
    }

    // Delegating methods to the current state
    void FixedUpdate() => currentState.FixedUpdate();
    public void OnCollisionEnter(Collision collision) => currentState.OnCollisionEnter(collision);
    public void OnCollisionExit(Collision collision) => currentState.OnCollisionExit(collision);

    private void GroundCheck()
    {
        // prevent ground check soon after jump
        if (rb.linearVelocity.y > playerStatistic.JumpForce.GetValue() / 2.0f) return;

        groundCheckDistance = rb.GetComponent<Collider>().bounds.extents.y;
        Vector3[] raycastOffsets = new Vector3[]
        {
            Vector3.zero,
            new (0, 0, rb.GetComponent<Collider>().bounds.extents.z),
            new (0, 0, -rb.GetComponent<Collider>().bounds.extents.z),
            new (rb.GetComponent<Collider>().bounds.extents.x, 0, 0),
            new (-rb.GetComponent<Collider>().bounds.extents.x, 0, 0),
        };

        foreach (var offset in raycastOffsets)
        {
            var raycastPosition = rb.position + offset;
            if (!Physics.Raycast(raycastPosition, Vector3.down, out var hit, this.groundCheckDistance)) continue;
            if (hit.collider.gameObject.CompareTag("Player")) continue;
            if (!(Vector3.Angle(Vector3.up, hit.normal) < this.groundCheckAngle)) continue;

            this.currentJumps = 0;
            if (!this.isGrounded) this.Tail.attackIndex = 0;

            this.isGrounded = true;
            this.playerAnimationsHandler.SetBool("IsOnGround", true);
            return;
        }

        if (!isGrounded) return;

        isGrounded = false;
        IsLanding = false;
        Tail.attackIndex = 0;
        playerAnimationsHandler.SetBool("IsOnGround", false);
    }

    private void CheckLandingAnimation()
    {
        if (rb.linearVelocity.y < -0.1f && isGrounded && !IsLanding)
        {
            IsLanding = true;
            playerAnimationsHandler.ResetStates();
            playerAnimationsHandler.animator.SetTrigger("IsLanding");
            playerAnimationsHandler.animator.ResetTrigger("IsJumping");
        }
    }

    public void SetState(StateBase newState)
    {
        if (currentState == states.Death) return;
        if (currentState == states.Attacking && newState == states.Attacking) return;
        // stop active coroutine
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        // chance state
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        // storing current name for debugging
        debug_currentStateName = currentState.GetType().Name;
        debug_lineColor = Color.yellow;
    }

    public IEnumerator SetStateAfter(StateBase newState, float time, bool override_ = false)
    {
        // stop active coroutine
        if (activeCoroutine != null)
        {
            if (override_)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
            else yield break;
        }

        // set state after time
        yield return new WaitForSeconds(time);
        activeCoroutine = null;
        SetState(newState);
    }

    public Vector3 GetDirection()
    {
        var moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
        return moveDirection.normalized;
    }

    private void RotatePlayerObj()
    {
        if (isKnockedBack && targetVelocity.magnitude < 0.1f) isKnockedBack = false;
        if (isKnockedBack) return;
        if (!(this.targetVelocity.magnitude > 0.1f)) return;

        var direction = Vector3.ProjectOnPlane(this.targetVelocity, Vector3.up).normalized;
        if (direction != Vector3.zero) this.rb.MoveRotation(Quaternion.Lerp(this.rb.rotation, Quaternion.LookRotation(direction), 50f * Time.deltaTime));
    }

    private void ApproachTargetVelocity()
    {
        // slowly move to target velocity
        var newVelocity = Vector3.MoveTowards(rb.linearVelocity, targetVelocity, currentMovementLerpSpeed * Time.deltaTime);

        // adjust speed when slowing down
        if (newVelocity.sqrMagnitude < rb.linearVelocity.sqrMagnitude)
        {
            // preserve y velocity
            var yVal = newVelocity.y;

            // apply deceleration
            var adjustedVelocity = newVelocity.normalized * (rb.linearVelocity.magnitude * (-0.01f * (currentState == states.DodgeRoll ? dodgeRollDeceleration : deceleration) + 1));
            if (adjustedVelocity.sqrMagnitude < newVelocity.sqrMagnitude) adjustedVelocity = newVelocity;

            // apply adjustment
            newVelocity = new(adjustedVelocity.x, yVal, adjustedVelocity.z);
        }

        // apply new velocity
        rb.linearVelocity = newVelocity;
    }

    public void UpdateVisualState()
    {
        var strength = (1 - playerStatistic.Health / playerStatistic.MaxHealth.GetValue()) * 0.5f + 0.2f;
        currentMoldPercentage -= (currentMoldPercentage - strength) * 2 * Time.deltaTime;

        foreach (var mat in mr.materials)
        {
            mat.SetFloat("_MoldStrength", currentMoldPercentage);
        }
        foreach (var mat in tailmr.materials)
        {
            mat.SetFloat("_MoldStrength", currentMoldPercentage);
        }
    }

    public void OnHit(float damage, Vector3 direction, DamageCause cause)
    {
        // update damage cause
        lastDamageCause = cause;

        // check if bradley should be invincible
        if (CheatCodeSystem.InvulnerableCheatActivated) return;
        if (roomInvulnerability) return;
        if (isInvulnerable) return;

        // check if bradley is dead
        if (currentState == states.Death) return;

        if (direction != Vector3.zero) currentState.Hurt(direction);

        GlobalReference.GetReference<AudioManager>().PlaySFX("PainSFX");

        playerAnimationsHandler.animator.SetTrigger("PlayDamageFlash"); // why is this wrapped, but does not implement all animator params?
        playerStatistic.Health -= damage;
        if (playerStatistic.Health <= 0) OnDeath();
        GlobalReference.AttemptInvoke(Events.HEALTH_CHANGED);
    }

    private IEnumerator InvulnerabilityTimer()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    public void ApplyKnockback(Vector3 knockback, float time)
    {
        isKnockedBack = true;
        rb.linearVelocity = knockback;
        StartCoroutine(KnockbackStunRoutine(time));
        // above is temporary
    }

    public void Heal(float reward)
    {
        playerStatistic.Health += reward;
        GlobalReference.AttemptInvoke(Events.HEALTH_CHANGED);
    }
    
    private void OnDeath()
    {
        CollectableSave saveData = new(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("level", GlobalReference.GetReference<GameManagerReference>().activeRoom.id);
        GlobalReference.GetReference<AudioManager>().PlaySFX("Death");
        saveData.LoadAll();
        SetState(states.Death);
    }

    public void FadeToDeathScreen()
    {
        StartCoroutine(DeathScreen());
    }

    private IEnumerator DeathScreen()
    {
        yield return StartCoroutine(crossfadeController.Crossfade_Start());
        SceneManager.LoadScene("Death");

        GlobalReference.Statistics.Increase("deaths", 1);
        AchievementManager.Grant("LIFE_IS_MOLDY");

        if (lastDamageCause == DamageCause.ENEMY) {
            GlobalReference.Statistics.Increase("death_by_enemy", 1);
        }
        else if (lastDamageCause == DamageCause.HAZARD) {
            GlobalReference.Statistics.Increase("death_by_hazard", 1);
            AchievementManager.Grant("SKILL_ISSUE");
        }
    }

    IEnumerator KnockbackStunRoutine(float time = 0.5f)
    {
        yield return new WaitForSecondsRealtime(time);
        isKnockedBack = false;
    }

    public void SetRandomFeedback()
    {
        succesfullHitCounter = 0;
        var f = rb.gameObject.GetComponentInChildren<FeedbackManager>();
        f.SetRandomFeedback();
    }

}

public enum DamageCause{
    NONE,
    ENEMY,
    HAZARD,
    PLAYER
}