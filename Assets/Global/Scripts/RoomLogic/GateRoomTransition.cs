using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateRoomTransition : Interactable
{
    [Header("Materials")]
    [SerializeField] private GameObject portalEffect;
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private string nextRoomName;
    [SerializeField] private int nextRoomIndex;
    public RoomType nextRoomType; // made public for structure change
    public int nextRoomId; // made public for structure change
    [SerializeField] private bool enableOnRoomFinish;
    private bool showCredits = false;
    private bool showSpeedRunResults = false;
    private bool loadingNextRoom = false;
    
    private GameManagerReference gameManagerReference;
    private DoorManager doorManager;
    private string currentSceneName;
    
    private void Awake()
    {
        // ugly fix so maybe we have to change in the future
        IsDisabled = IsDisabled; // However, don't remove it until we changed it.
    }

    public override void Start()
    {
        base.Start();
        var crossFadeController = GlobalReference.GetReference<CrossFadeController>();
        StartCoroutine(crossFadeController.CrossFadeEnd());
    }

    public void Initialize()
    {
        gameManagerReference = GlobalReference.GetReference<GameManagerReference>();

        if (enableOnRoomFinish) GlobalReference.SubscribeTo(Events.ROOM_FINISHED, RoomFinished);
        else IsDisabled = !gameManagerReference.GetRoom(nextRoomId).unlocked;

        doorManager = GlobalReference.GetReference<DoorManager>();
        nextRoomName = $"{nextRoomType}_{nextRoomIndex}";
    }

    private void RoomFinished()
    {
        IsDisabled = false;
    }

    public override void OnInteract(ActionMetaData _)
    {
        // Intentionally put the sound above guard clause. Even though interacting does nothing when spamming it.
        // It is still to give the player feedback that the interaction is being registered.
        GlobalReference.GetReference<AudioManager>().PlaySFX("PortalSFX");
        if (loadingNextRoom) return;
        
        // unlock next level
        GlobalReference.AttemptInvoke(Events.ROOM_FINISHED);
        var nextLevel = gameManagerReference.GetRoom(gameManagerReference.activeRoom.id + 1);
        if (nextLevel == null) AchievementManager.Grant("RISE_OF_THE_LOAF");
        else nextLevel.unlocked = true;
        
        StartCoroutine(LoadNextRoom());
        var p = GlobalReference.GetReference<PlayerReference>().Player;
        p.SetCameraHeight(null); // height reset to default
        p.Heal(p.playerStatistic.MaxHealth.GetValue());
        
        base.OnInteract(_);
    }
    
    private IEnumerator LoadNextRoom()
    {
        loadingNextRoom = true;
        var crossFadeController = GlobalReference.GetReference<CrossFadeController>();
        yield return StartCoroutine(crossFadeController.CrossFadeStart());
        
        SaveCurrentRoomsProgress();
        
        // Since rooms only have one exit door, and exiting a room through this door is the 'completion' condition. We can assume that if a door with the NextRoomType: ENTRANCE is an exit door to the hub.
        if (nextRoomType == RoomType.ENTRANCE) SaveRoomAsCompleted();

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name != "BaseScene" && scene.isLoaded)
                currentSceneName = scene.name;
        }

        RetrieveNextRoomCalorieStat();
        
        if (showCredits)
        {
            SceneManager.LoadScene("Credits");
        }
        else if (showSpeedRunResults) {
            SceneManager.LoadScene("SpeedrunResults");
        }
        else
        {
            SceneManager.LoadScene(nextRoomName, LoadSceneMode.Additive);

            var gameManagerReference = GlobalReference.GetReference<GameManagerReference>();
            if (gameManagerReference != null)
            {
                doorManager.currentId = nextRoomId;
                var nextRoom = gameManagerReference.GetRoom(nextRoomId);
                if (nextRoom != null)
                {
                    gameManagerReference.activeRoom = nextRoom;
                }
                else
                {
                    Debug.LogError($"Failed to find Room with Type: {nextRoomType}");
                }
            }
            else
            {
                Debug.LogError("GameManagerReference is null");
            }

            var unloadOperation = SceneManager.UnloadSceneAsync(currentSceneName);
            while (!unloadOperation.isDone)
            {
                yield return null;
            }
            var newScene = SceneManager.GetSceneByName(nextRoomName);
            SceneManager.SetActiveScene(newScene);
        }
    }

    private void SaveCurrentRoomsProgress()
    {
        var player = GlobalReference.GetReference<PlayerReference>().Player;

        var saveData = new CollectableSave(GetNonBaseSceneName("BaseScene"));
        saveData.LoadAll();
        var calories = saveData.Get<List<string>>("calories");
        if (saveData.Get<int>("crumbs") < player.playerStatistic.Crumbs)
            saveData.Set("crumbs", player.playerStatistic.Crumbs);

        foreach (var secret in player.playerStatistic.Calories)
        {
            calories.Add(secret);
        }
        saveData.Set("calories", calories);
        saveData.SaveAll();

        //to reset everything that was picked up
        player.playerStatistic.CaloriesCount = 0;
        player.playerStatistic.CrumbsCount = 0;
        player.playerStatistic.Calories.Clear();
        player.playerStatistic.Crumbs = 0;
    }

    private void RetrieveNextRoomCalorieStat()
    {
        var player = GlobalReference.GetReference<PlayerReference>().Player;
        var saveData = new CollectableSave(nextRoomName);
        saveData.LoadAll();
        player.playerStatistic.caloriesCountExtra = saveData.Get<List<string>>("calories").Count;
        player.playerStatistic.CaloriesCollected = saveData.Get<List<string>>("calories");

    }

    public override void OnDisableInteraction()
    {
        portalEffect?.SetActive(false);
        // TODO: implement closing animations
        //  Not a problem though, since this method only gets called when using cheats
    }

    public override void OnEnableInteraction()
    {
        portalEffect?.SetActive(true);
        animator.SetTrigger("TriggerDoorOpen");
        animator.SetTrigger("TriggerDoorRight");
    }

    private void OpenDoorAnimation()
    {
        if (IsDisabled) return;
        animator.SetTrigger("TriggerDoorOpen");
        animator.SetTrigger("TriggerDoorRight");
    }

    private string GetNonBaseSceneName(string baseSceneName)
    {
        return Enumerable.Range(0, SceneManager.sceneCount)
            .Select(i => SceneManager.GetSceneAt(i))
            .FirstOrDefault(scene => scene.name != baseSceneName && scene.isLoaded).name ?? baseSceneName;
    }


    private void SaveRoomAsCompleted()
    {
        var saveRoomData = new RoomSave();
        saveRoomData.LoadAll();
        var finishedLevels = saveRoomData.Get<List<int>>("finishedLevels");
        showCredits = !IsSpeedrunMode() && doorManager.currentId == 3 && !finishedLevels.Contains(3);
        showSpeedRunResults = IsSpeedrunMode() && doorManager.currentId == 3;
        finishedLevels.Add(doorManager.currentId);
        saveRoomData.Set("finishedLevels", finishedLevels);
        saveRoomData.SaveAll();
    }

    bool IsSpeedrunMode() => GlobalReference.Settings.Get<bool>("speedrun_mode");
 
    [ContextMenu("Unlock Door")] public void UnlockDoor() => IsDisabled = false;
    [ContextMenu("Lock Door")] void LockDoorTest() => IsDisabled = true;
    [ContextMenu("Open Door")] void OpenDoorTest() => OpenDoorAnimation();
    [ContextMenu("Invoke room finish event")] void InvoteRoomFinishedEvent() => GlobalReference.AttemptInvoke(Events.ROOM_FINISHED);
}
