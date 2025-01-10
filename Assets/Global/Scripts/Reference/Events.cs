using UnityEngine;

public enum Events
{
    // IMPORTANT: DON'T CHANGE THE NUMBERS OF EXISTING ENUMS. JUST ADD NEW ONES WITH A NEW NON EXISTING NUMBER
    #region general
    PICKUP_COLLECTED = 0,
    ROOM_FINISHED = 1,
    DROP_TREASURE_EVENT = 2,
    #endregion

    #region enemy
    ENEMY_KILLED = 3,
    ENEMY_SPAWNED = 4,
    #endregion

    #region waves
    ALL_ENEMIES_DEAD = 5,
    NORMAL_WAVE_DONE = 6,
    NORMAL_WAVE_START = 7,
    SPAWN_WAVE = 8,
    ALL_WAVES_DONE = 9,
    MOLD_CORE_SPAWNED = 10,
    MOLD_CORE_KILLED = 11,
    NEXT_SPAWNER = 12,
    ALL_NEXT_SPAWNERS_DONE = 13,
    #endregion

    // STATS RELATED EVENTS
    #region stats
    CRUMBS_CHANGED = 14,
    HEALTH_CHANGED = 15,
    STATISTIC_CHANGED = 16,
    MOLDMETER_CHANGED = 17,
    #endregion

    // player attacks
    #region player attack
    PLAYER_ATTACK_STARTED = 18,
    PLAYER_ATTACK_ENDED = 19,
    #endregion
    #region enemy attack
    ENEMY_ATTACK_STARTED = 20,
    ENEMY_ATTACK_ENDED = 21,
    ENEMY_TO_IDLE = 22,
    #endregion

    //player death
    PLAYER_DIED = 23
}
