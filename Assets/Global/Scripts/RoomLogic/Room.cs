using System.Collections.Generic;

public class Room
{
    // field
    public int id;
    public RoomType roomType;
    public bool unlocked;
    public bool finished;

    // constructor
    public Room(int id_, RoomType roomType_, bool unlocked_ = false)
    {
        id = id_;
        roomType = roomType_;
        unlocked = unlocked_;
        finished = false;
    }

    public bool IsStandard()
    {
        return roomType switch
        {
            RoomType.COMBAT => true,
            RoomType.PARKOUR => true,
            RoomType.MOLDORB => true,
            RoomType.WAVESURVIVAL => true,
            _ => false,
        };
    }

    // static info
    public static Dictionary<RoomType, int> RoomDistribution = new() {
        {RoomType.COMBAT, 90},
        {RoomType.PARKOUR, 0},
        {RoomType.MOLDORB, 0},
        {RoomType.WAVESURVIVAL, 0}
    };
}

public enum RoomType
{
    OTHER,      // anything non specified, preferably shouldn't be used
    ENTRANCE,   // entrance room, first room where the player starts the game
    EXIT,       // exit room, last room on a certain floor
    SHOP,       // shop, where the player can buy stuff
    BONUS,      // hidden treasure room that will redirect back to the previous room 
    COMBAT,     // room with a focus on combat
    PARKOUR,    // room with a focus on movement
    MOLDORB,    // room that contains mold orbs that will need to be destroyed
    WAVESURVIVAL,// room that spawn several waves of enemies
    SPEEDRUNRESULTS,
    CREDITS
}