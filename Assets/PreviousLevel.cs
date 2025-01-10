using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousLevel : MonoBehaviour
{
    public static PreviousLevel Instance { get; private set; }
    public int prevLevel = -1;

    protected void Awake()
    {
        // Check if an instance of this script already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GlobalReference.SubscribeTo(Events.PLAYER_DIED, SetPreviousLevel);
    }

    private void SetPreviousLevel()
    {
        prevLevel = SceneManager.GetSceneAt(1).buildIndex; // i hate how not safe this is but yeah [0] = basescene, [1] = level
    }

    public void ResetPreviousLevel()
    {
        prevLevel = -1;
    }
}
