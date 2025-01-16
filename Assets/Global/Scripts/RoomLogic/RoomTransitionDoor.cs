using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class RoomTransitionDoor : Interactable
{
    [Header("Materials")]
    [SerializeField] private GameObject portalEffect;
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private string nextRoomName;
    [SerializeField] private int nextRoomIndex;
    public RoomType nextRoomType; // made public for structure change
    public int nextRoomId; // made public for structure change
    private int roomAmounts;
    [SerializeField] private bool enableOnRoomFinish;

    private GameManagerReference gameManagerReference;
    private CrossfadeController crossfadeController;
    private DoorManager doorManager;

    private string currentScenename;
    private void Awake()
    {
        IsDisabled = IsDisabled; // ugly fix so maybe we have to change in the future
        crossfadeController = GlobalReference.GetReference<CrossfadeController>();
    }

    public void Initialize()
    {
        gameManagerReference = GlobalReference.GetReference<GameManagerReference>();

        if (enableOnRoomFinish) GlobalReference.SubscribeTo(Events.ROOM_FINISHED, RoomFinished);
        else IsDisabled = !gameManagerReference.GetRoom(nextRoomId).unlocked;

        doorManager = GlobalReference.GetReference<DoorManager>();
        roomAmounts = gameManagerReference.GetAmountForRoomType(nextRoomType);
        // int randomIndex = Random.Range(1, roomAmounts + 1); // disabled for structure change
        nextRoomName = nextRoomType.ToString() + "_" + nextRoomIndex;
    }

    private void RoomFinished()
    {
        IsDisabled = false;
    }

    public override void OnInteract(ActionMetaData _)
    {
        // unlock next level
        Room nextLevel = gameManagerReference.GetRoom(gameManagerReference.activeRoom.id + 1);
        if (nextLevel.id >= gameManagerReference.GetRooms().Count()) AchievementManager.Grant("RISE_OF_THE_LOAF");
        else if (nextLevel != null) nextLevel.unlocked = true;
        AudioManager.Instance.PlaySFX("PortalSFX");
        StartCoroutine(LoadNextRoom());
        Player p = GlobalReference.GetReference<PlayerReference>().Player;
        p.setCameraHeight(null); // height reset to default
        p.Heal(p.playerStatistic.MaxHealth.GetValue());
    }

    private IEnumerator LoadNextRoom()
    {
        yield return StartCoroutine(crossfadeController.Crossfade_Start());
        yield return StartCoroutine(LoadRoomCoroutine());
    }

    public IEnumerator LoadRoomCoroutine()
    {
        var player = GlobalReference.GetReference<PlayerReference>().Player;

        CollectableSave saveData = new CollectableSave(GetNonBaseSceneName("BaseScene"));
        saveData.LoadAll();
        List<string> calories = saveData.Get<List<string>>("calories");
        if (saveData.Get<int>("crumbs") < player.playerStatistic.Crumbs)
        {
            saveData.Set("crumbs", player.playerStatistic.Crumbs);
        }

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

        // Since rooms only have one exit door, and exiting a room through this door is the 'completion' condition. We can assume that if a door with the NextRoomType: ENTRANCE is an exit door to the hub.
        if (nextRoomType == RoomType.ENTRANCE) SaveRoomAsCompleted();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name != "BaseScene" && scene.isLoaded)
            {
                currentScenename = scene.name;
            }
        }

        Debug.Log($"next: {nextRoomName}, nextId: {nextRoomId}, nextIndex: {nextRoomIndex}");
        saveData = new CollectableSave(nextRoomName);
        saveData.LoadAll();
        player.playerStatistic.caloriesCountExtra = saveData.Get<List<string>>("calories").Count;
        player.playerStatistic.CaloriesCollected = saveData.Get<List<string>>("calories");
        SceneManager.LoadScene(nextRoomName, LoadSceneMode.Additive);

        var gameManagerReference = GlobalReference.GetReference<GameManagerReference>();
        if (gameManagerReference != null)
        {
            doorManager.currentId = nextRoomId;
            Room nextRoom = gameManagerReference.GetRoom(nextRoomId);
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

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScenename);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }
        Scene newScene = SceneManager.GetSceneByName(nextRoomName);
        SceneManager.SetActiveScene(newScene);
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
        RoomSave saveRoomData = new RoomSave();
        saveRoomData.LoadAll();
        List<int> finishedLevels = saveRoomData.Get<List<int>>("finishedLevels");
        finishedLevels.Add(doorManager.currentId);
        saveRoomData.Set("finishedLevels", finishedLevels);
        saveRoomData.SaveAll();
    }

    [ContextMenu("Unlock Door")] public void UnlockDoor() => IsDisabled = false;
    [ContextMenu("Lock Door")] void LockDoorTest() => IsDisabled = true;
    [ContextMenu("Open Door")] void OpenDoorTest() => OpenDoorAnimation();
    [ContextMenu("Invoke room finish event")] void InvoteRoomFinishedEvent() => GlobalReference.AttemptInvoke(Events.ROOM_FINISHED);
}
