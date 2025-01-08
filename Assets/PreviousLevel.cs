using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousLevel : Reference
{
    public int prevLevel;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GlobalReference.SubscribeTo(Events.PLAYER_DIED, SetPreviousLevel);
    }

    private void SetPreviousLevel()
    {
        prevLevel = SceneManager.GetSceneAt(1).buildIndex; // i hate how not safe this is but eh
        Debug.Log($"prevLevel set to: {prevLevel}");
    }
}
