using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousLevel : MonoBehaviour
{
    public static PreviousLevel Instance { get; private set; }
    public int prevLevel = -1;
    public int prevLevelId = 0;

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
        GlobalReference.SubscribeTo(Events.PLAYER_DIED, this.SetLevelForRetry);
    }

    private void SetLevelForRetry()
    {
        prevLevel = SceneManager.GetSceneAt(1).buildIndex; // i hate how not safe this is but yeah [0] = basescene, [1] = level
        prevLevelId = GlobalReference.GetReference<DoorManager>().currentId;
    }

    public void ResetLevelForRetry()
    {
        prevLevel = -1;
    }
}
