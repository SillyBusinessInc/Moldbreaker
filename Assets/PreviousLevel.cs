using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousLevel : Reference
{
    public static PreviousLevel Instance { get; private set; } // Singleton instance
    public int prevLevel;

    protected override void Awake()
    {
        // Check if an instance of this script already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    void Start()
    {
        GlobalReference.SubscribeTo(Events.PLAYER_DIED, SetPreviousLevel);
    }

    private void SetPreviousLevel()
    {
        prevLevel = SceneManager.GetSceneAt(1).buildIndex; // i hate how not safe this is but yeah [0] = basescene, [1] = level
        Debug.Log($"prevLevel set to: {prevLevel}");
    }
}
