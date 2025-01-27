using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class CheatCodeSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    public float maxComboTime = 4f;
    private float comboTimer;
    public static bool InvulnerableCheatActivated = false;
    private Dictionary<string, Action> cheatCodes = new Dictionary<string, Action>
    { // Add your cheat codes here
        { "LULDR", InvokeInfiniteDoubleJumps },
        { "LDRLU", InvokeExtraSpeed },
        { "RLLRD", InvokeEnableDodge },
        { "UUDLR", InvokeInstantDeath },
        { "DDRLU", InvokeRestoreFullHp },
        { "UDLRUD", InvokeToggleInvulnerability },
        { "UDLRRLDD", InvokeEnableAllLevels },
        { "UUDDLRLR", () => AchievementManager.Grant("KONAMI_CODE") }
    }; 
    
    [Header("Debugging")]
    [SerializeField] private string currentSequence = "";
#pragma warning disable 0414
    // Yes its true that its never used. it's only here to show it in the inspector for debugging purposes
    [SerializeField] private string LastInvokedCheat = "none";
#pragma warning restore 0414
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["LeftKey"].performed += OnLeftKey;
        playerInput.actions["RightKey"].performed += OnRightKey;
        playerInput.actions["DownKey"].performed += OnDownKey;
        playerInput.actions["UpKey"].performed += OnUpKey;
    }

    private void OnDisable()
    {
        playerInput.actions["LeftKey"].performed -= OnLeftKey;
        playerInput.actions["RightKey"].performed -= OnRightKey;
        playerInput.actions["DownKey"].performed -= OnDownKey;
        playerInput.actions["UpKey"].performed -= OnUpKey;
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(currentSequence)) return;

        comboTimer += Time.deltaTime;
        if (comboTimer > maxComboTime)
            ResetCombo();
    }

    private void AddToSequence(string key)
    {
        currentSequence += key;
        CheckSequence();
    }
    
    private void OnLeftKey(InputAction.CallbackContext context) => AddToSequence("L");
    private void OnRightKey(InputAction.CallbackContext context) => AddToSequence("R");
    private void OnDownKey(InputAction.CallbackContext context) => AddToSequence("D");
    private void OnUpKey(InputAction.CallbackContext context) => AddToSequence("U");

    private void CheckSequence()
    {
        if (cheatCodes.ContainsKey(currentSequence))
        {
            LastInvokedCheat = currentSequence;
            cheatCodes[currentSequence].Invoke();
            
            var player = GlobalReference.GetReference<PlayerReference>().Player;
            player.PlayVFX();
            GlobalReference.GetReference<AudioManager>().PlaySFX("Cheat");

            ResetCombo();
            return;
        }
        
        // If we did not found a match, we want to make sure that there is still a possible match
        // If not, we reset the combo
        if (cheatCodes.Keys.Select(x => x.StartsWith(currentSequence)).Contains(true))
            return; // there is still a combo to be made, so no reset
        
        ResetCombo();
    }

    private void ResetCombo()
    {
        currentSequence = "";
        comboTimer = 0;
    }
    
    // -=-
    // Cheat Code Invocations
    // -=-

    private static void InvokeToggleInvulnerability() => InvulnerableCheatActivated = !InvulnerableCheatActivated; 
    private static void InvokeInfiniteDoubleJumps() => GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.DoubleJumpsCount.AddModifier("cheatleg", 1000);

    private static void InvokeEnableDodge() => GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CanDodge.AddModifier("cheatdodge", 1);
    
    private static void InvokeRestoreFullHp() => GlobalReference.GetReference<PlayerReference>().Player.Heal(GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.MaxHealth.GetValue());
    
    private static void InvokeExtraSpeed()
    {
        var stats = GlobalReference.GetReference<PlayerReference>().Player.playerStatistic;
        stats.Speed.AddModifier("cheatspeed", 8);  
        stats.JumpForce.AddModifier("cheatspeed", 4);  
    } 
    
    private static void InvokeInstantDeath()
    {
        InvulnerableCheatActivated = false;
        GlobalReference.GetReference<PlayerReference>().Player.OnHit(float.MaxValue, Vector3.zero, DamageCause.NONE);
    }
    
    private static void InvokeEnableAllLevels()
    {
        // adding asif we completed 25 levels. There are no 25 levels in the game,
        // it's just to be extremely future proof
        var myList = new List<int>();
        for (var i = 0; i < 25; i++)
        {
            myList.Add(i);
        }
        
        RoomSave saveRoomData = new();
        saveRoomData.Set("finishedLevels", myList);
        saveRoomData.SaveAll();
        GlobalReference.AttemptInvoke(Events.LEVELS_CHANGED_BY_CHEAT);
    }
}
