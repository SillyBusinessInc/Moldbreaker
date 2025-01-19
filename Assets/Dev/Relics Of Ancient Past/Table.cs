using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using SRandom = System.Random;

public class Table
{
    // Settings
    private int maxObjectPerDepth;
    private int minBranchCount;
    private int maxBranchCount;
    private int targetDepth;
    private int bonusChance;
    private int seed;
    private int shopDepthOverride;

    // Content
    private int shopDepth = -1;
    public List<Row> table;
    private SRandom random;

    // Constructor
    public Table() => Generate();

    // Data
    public Row GetRow(int id) => table.Where(x => x.id == id).FirstOrDefault();
    public Row GetFirstUnpopulatedRow() => table.Where(x => x.branches.Count == 0).FirstOrDefault(); 
    public int GetNextId() => table?.Count ?? 0;
    public int GetIndexInDepth(Row row) => GetRowsAtDepth(row.depth).OrderBy(x => x.id).ToList().FindIndex(x => x.id == row.id);
    public List<int> GetDepthColumn() => table.Select(x => x.depth).ToList();
    public List<Row> GetRowsAtDepth(int depth) => table.Where(x => x.depth == depth && GlobalReference.GetReference<GameManagerReference>().GetRoom(x.id).roomType != RoomType.BONUS).ToList();

    // Reference
    public string RowReference(Row row) => $"Row: {row.id}, {row.depth}, [{string.Join(';', row.branches)}]";

    // Generation Logic
    public void Generate() {
        // importing settings
        maxObjectPerDepth = GlobalReference.DevSettings.Get<int>("maxObjectPerDepth");
        minBranchCount = GlobalReference.DevSettings.Get<int>("minBranchCount");
        maxBranchCount = GlobalReference.DevSettings.Get<int>("maxBranchCount");
        targetDepth = GlobalReference.DevSettings.Get<int>("targetDepth");
        bonusChance = GlobalReference.DevSettings.Get<int>("bonusChance");
        seed = GlobalReference.DevSettings.Get<int>("seed");
        shopDepthOverride = GlobalReference.DevSettings.Get<int>("shopDepthOverride");
        // GlobalReference.DevSettings.ListAll<int>();

        // initialization
        if (seed < 0) random = new(Guid.NewGuid().GetHashCode());
        else random = new(seed);

        shopDepth = shopDepthOverride >= -1 ? shopDepth : random.Next(2, targetDepth);

        GlobalReference.GetReference<GameManagerReference>().ResetRooms();

        table = new();
        GetIdFromNewRow(0, RoomType.ENTRANCE);

        Populate(GetFirstUnpopulatedRow());
    }
    
    private void Populate(Row row) {
        // base cases
        if (row.depth == targetDepth) return;

        // init
        RoomType roomType = RoomType.OTHER;
        var isStandard = GlobalReference.GetReference<GameManagerReference>().GetRoom(row.id).IsStandard();
        if (isStandard && random.Next(0, 100) < bonusChance) AddBonusRoom(row);

        // get total branches to apply
        int branchesToApply = random.Next(minBranchCount, maxBranchCount+1);
        // get list of all existing branches at target depth
        List<Row> existingBranches = GetRowsAtDepth(row.depth+1);
        // calculate the room for new rooms taking into account the existing amount of branches and the max amount of branches
        int roomForNewBranches = maxObjectPerDepth - existingBranches.Count();
        // calculating the amount of already existing rooms we can branch into
        int roomForOldBranches = existingBranches.Count();

        // deciding the amount of new rooms to create
        int newBranchesToApply = random.Next(0, Math.Min(roomForNewBranches, branchesToApply) + 1);
        // deciding the amount of existing rooms to branch into
        int oldBranchesToApply = existingBranches.Count() != 0 ? Math.Min(roomForOldBranches, branchesToApply - newBranchesToApply) : 0;
        // destribute leftover branches
        int leftoverBranchesToApply = branchesToApply - newBranchesToApply - oldBranchesToApply;
        if (leftoverBranchesToApply > 0) newBranchesToApply = Mathf.Min(newBranchesToApply + leftoverBranchesToApply, roomForNewBranches);

        // define room type
        if (row.depth+1 == targetDepth ) {
            roomType = RoomType.EXIT;
            if (existingBranches.Count() == 0) {
                newBranchesToApply = 1;
                oldBranchesToApply = 0;
            }
            else {
                newBranchesToApply = 0;
                oldBranchesToApply = 1;
            }
        }
        else if (row.depth+1 == shopDepth ) {
            roomType = RoomType.SHOP;
            if (existingBranches.Count() == 0) {
                newBranchesToApply = 1;
                oldBranchesToApply = 0;
            }
            else {
                newBranchesToApply = 0;
                oldBranchesToApply = 1;
            }
        }

        // Debug.Log($"row: {row.id} | total: {branchesToApply} | new: {newBranchesToApply} | old: {oldBranchesToApply} | room: {roomForNewBranches}");

        // create branches
        for (int i = 0; i < newBranchesToApply; i++ ) {
            row.branches.Add(GetIdFromNewRow(row.depth+1, roomType));
        }
        for (int i = 0; i < oldBranchesToApply; i++ ) {
            row.branches.Add(GetIdFromOldRow(existingBranches));
        }

        // recursion
        Populate(GetFirstUnpopulatedRow());
    }
    public void AddBonusRoom(Row targetRow) {
        targetRow.branches.Add(GetIdFromNewRow(targetRow.depth, RoomType.BONUS, new() {targetRow.id}));
    }

    private int GetIdFromNewRow(int depth, RoomType roomType = RoomType.OTHER, List<int> branches = null) {
        Row newRow = new(GetNextId(), depth, branches);
        table.Add(newRow);

        if (roomType == RoomType.OTHER) roomType = RandomDistribution.GetRandom(Room.RoomDistribution, random);
        GlobalReference.GetReference<GameManagerReference>().AddRoom(newRow.id, roomType);
        return newRow.id;
    }

    private int GetIdFromOldRow(List<Row> existingBranches) {
        Row newRow = existingBranches[random.Next(0, existingBranches.Count)];
        existingBranches.Remove(newRow);
        return newRow.id;
    }

    // // Debug print Table (Tree Structure)
    // public void PrintTableAsTree() {
    //     Debug.Log("Generated Table (Tree Structure):");
    //     PrintRowRecursive(GetRow(0), 0);
    // }

    // private void PrintRowRecursive(Row row, int indentLevel) {
    //     string indent = new string(' ', indentLevel * 4);
    //     Debug.Log($"{indent}- Row {row.id} (Depth: {row.depth})");

    //     foreach (int branchId in row.branches) {
    //         Row branch = GetRow(branchId);
    //         PrintRowRecursive(branch, indentLevel + 1);
    //     }
    // }
}

public struct Row {
    public int id;
    public int depth;
    public int column;
    public List<int> branches;

    public Row(int id_, int depth_, List<int> branches_, int column_ = -1) {
        id = id_;
        depth = depth_;
        column = column_;
        branches = branches_ ?? new List<int>();
    }
}
