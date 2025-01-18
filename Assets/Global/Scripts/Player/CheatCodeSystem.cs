using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CheatCodeSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    public float maxComboTime = 4f; // Max time to complete the combo
    private float comboTimer;
    [HideInInspector] public bool InvulnerableCheatActivated = false;
    private List<string> cheatCodes = new List<string> { "LULDR", "RLLRD", "UUDLR", "DDRLU", "UDLRRLDD", "UDLRUDUD" }; // Example sequences

    [Header("Debug")]
    [SerializeField] private string currentSequence = "";
    [SerializeField] private string LastInvokedCheat = "none";
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Subscribe to input actions
        playerInput.actions["LeftKey"].performed += OnLeftKey;
        playerInput.actions["RightKey"].performed += OnRightKey;
        playerInput.actions["DownKey"].performed += OnDownKey;
        playerInput.actions["UpKey"].performed += OnUpKey;
    }

    private void OnDisable()
    {
        // Unsubscribe from input actions
        playerInput.actions["LeftKey"].performed -= OnLeftKey;
        playerInput.actions["RightKey"].performed -= OnRightKey;
        playerInput.actions["DownKey"].performed -= OnDownKey;
        playerInput.actions["UpKey"].performed -= OnUpKey;
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(currentSequence))
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > maxComboTime)
            {
                ResetCombo();
            }
        }
    }

    private void OnLeftKey(InputAction.CallbackContext context)
    {
        currentSequence += "L";
        CheckSequence();
    }

    private void OnRightKey(InputAction.CallbackContext context)
    {
        currentSequence += "R";
        CheckSequence();
    }

    private void OnDownKey(InputAction.CallbackContext context)
    {
        currentSequence += "D";
        CheckSequence();
    }

    private void OnUpKey(InputAction.CallbackContext context)
    {
        currentSequence += "U";
        CheckSequence();
    }

    private void CheckSequence()
    {
        foreach (var cheat in cheatCodes)
        {
            if (currentSequence == cheat)
            {
                ActivateCheat(cheat);
                ResetCombo();
                return;
            }
        }

        bool match = false;
        foreach (var cheat in cheatCodes)
        {
            if (cheat.StartsWith(currentSequence))
            {
                match = true;
                break;
            }
        }

        if (!match)
        {
            ResetCombo();
        }
    }

    private void ActivateCheat(string cheat)
    {
        switch (cheat)
        {
            case "LULDR":
                //infinite double jump
                LastInvokedCheat = "Infinite Double jumps";
                GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.DoubleJumpsCount.AddModifier("cheatleg", 1000);
                break;
            case "RLLRD":
                LastInvokedCheat = "Enable Dodge";
                GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.CanDodge.AddModifier("cheatdodge", 1);
                break;
            case "UUDLR":
                InvulnerableCheatActivated = false;
                LastInvokedCheat = "Killing yourself";
                GlobalReference.GetReference<PlayerReference>().Player.OnHit(float.MaxValue, Vector3.zero);
                break;
            case "DDRLU":
                LastInvokedCheat = "Restoring Full HP";
                GlobalReference.GetReference<PlayerReference>().Player.Heal(GlobalReference.GetReference<PlayerReference>().Player.playerStatistic.MaxHealth.GetValue());
                break;
            case "UDLRRLDD":
                LastInvokedCheat = "Enable All Levels";
                EnableAllLevels();
                break;
            case "UDLRUDUD":
                LastInvokedCheat = "Invulnerability";
                ToggleInvulnerability();
                break;
        }
    }

    private void EnableAllLevels()
    {
        RoomSave saveRoomData = new();
        var myList = new List<int>();
        for (int i = 0; i < 25; i++)
        {
            myList.Add(i);
        }
        saveRoomData.Set("finishedLevels", myList);
        saveRoomData.SaveAll();
        GlobalReference.AttemptInvoke(Events.LEVELS_CHANGED);
    }
    
    private void ResetCombo()
    {
        currentSequence = "";
        comboTimer = 0;
        Debug.Log("Combo Reset");
    }

    private void ToggleInvulnerability()
    {
        InvulnerableCheatActivated = !InvulnerableCheatActivated;
    }
}
