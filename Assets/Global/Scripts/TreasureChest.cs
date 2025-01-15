using UnityEngine;

public class TreasureChestInteractable : Interactable
{
    [Header("Treasure Chest Settings")]
    [SerializeField] private LootTable lootTable;
    [Tooltip("The reward config it uses for this treasure chest [Read Only]")]
    [SerializeField] private RewardConfig rewardConfig;


    private GameManagerReference gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        if (rewardConfig == null)
        {
            Debug.Log("[Config] RewardConfig is not set in the inspector [TreasureChestInteractable]");
            return;
        }

        gameManager = GlobalReference.GetReference<GameManagerReference>();
        RoomType roomType = gameManager.GetRoom(gameManager.activeRoom.id).roomType;
        lootTable = rewardConfig.GetLootTableForRoomType(roomType);
    }

    public override void OnInteract(ActionMetaData metaData)
    {
        if (lootTable == null) return;

        base.OnInteract(metaData);

        var loot = lootTable.GetRandomReward();

        if (loot != null)
        {
            // loop through the list of actions and invoke them
            foreach (var action in loot.Entry) action.InvokeAction(metaData);
        }

        Destroy(this.gameObject);
    }
}
