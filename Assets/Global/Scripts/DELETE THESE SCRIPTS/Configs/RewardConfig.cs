using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/RewardConfig")]
public class RewardConfig : ScriptableObject
{
    [System.Serializable]
    public class EventLootTablePair
    {
        public List<RoomType> roomType;
        public LootTable lootTable;
    }

    [SerializeField] private List<EventLootTablePair> eventLootTablePairs;
    [SerializeField] private LootTable fallBackLootTable;

    private Dictionary<RoomType, LootTable> lookupTable;

    private void OnEnable()
    {
        // Precompute the lookup table for efficient runtime queries
        lookupTable = eventLootTablePairs
            .SelectMany(pair => pair.roomType.Select(room => new { room, pair.lootTable }))
            .ToDictionary(x => x.room, x => x.lootTable);
    }

    public LootTable GetLootTableForRoomType(RoomType roomType)
    {
        if (lookupTable != null && lookupTable.TryGetValue(roomType, out var lootTable))
        {
            return lootTable;
        }
        return fallBackLootTable;
    }
}
