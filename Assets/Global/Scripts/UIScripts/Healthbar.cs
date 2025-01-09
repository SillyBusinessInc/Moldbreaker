using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    private List<GameObject> healthObjects = null;

    [SerializeField] private GameObject healthPrefab;

    void Awake()
    {
        GlobalReference.SubscribeTo(Events.HEALTH_CHANGED, UpdateCurrentHealthByPlayer);
    }

    public void Initialize()
    {
        healthObjects = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            healthObjects.Add(transform.GetChild(i).gameObject);
        }
        Player player = GlobalReference.GetReference<PlayerReference>().Player;

        UpdateMaxHealth(player.playerStatistic.MaxHealth.GetValue());
        UpdateCurrentHealth(player.playerStatistic.Health);

        player.playerStatistic.MaxHealth.Subscribe(() => UpdateMaxHealth(player.playerStatistic.MaxHealth.GetValue()));
    }

    private void UpdateCurrentHealthByPlayer()
    {
        if (healthObjects == null) Initialize();
        else
        {
            Player player = GlobalReference.GetReference<PlayerReference>().Player;
            UpdateCurrentHealth(player.playerStatistic.Health);
        }
    }

    public void UpdateMaxHealth(float maxHealth)
    {
        // populate new health bar
        int newSize = (int)Mathf.Ceil(maxHealth / 2);
        int currentSize = healthObjects.Count;
        bool isHalf = maxHealth % 2 != 0;
        // Debug.Log($"Updating max health: {maxHealth}, newsize: {newSize}, currentsize: {currentSize}, half: {isHalf}");

        SetMode(healthObjects.LastOrDefault(), "full", true);

        // add items if increase
        if (currentSize < newSize)
        {
            for (int i = currentSize; i < newSize; i++)
            {
                GameObject newObj = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity);
                newObj.transform.SetParent(transform);
                RectTransform source = newObj.GetComponent<RectTransform>();
                source.anchorMin = new(i, 0);
                source.anchorMax = new(i + 1, 1);
                source.offsetMin = new(0, 0);
                source.offsetMax = new(0, 0);
                source.localScale = new(1, 1, 1);
                SetMode(newObj, "full", true);
                healthObjects.Add(newObj);
            }
        }
        // remove items if decreased
        else
        {
            int toRemove = currentSize - newSize;
            for (int i = 0; i < toRemove; i++)
            {
                GameObject oldObj = healthObjects.LastOrDefault();
                Destroy(oldObj);
                healthObjects.Remove(oldObj);
            }
        }

        // set last heart to half if applicable
        if (isHalf)
        {
            SetMode(healthObjects.LastOrDefault(), "half", true);
        }
    }

    public void UpdateCurrentHealth(float currentHealth)
    {
        // populate new health bar
        int newSize = (int)Mathf.Ceil(currentHealth / 2);
        int currentSize = healthObjects.Where((x) => x.transform.GetChild(1).gameObject.activeSelf).ToList().Count;
        bool isHalf = currentHealth % 2 != 0;
        // Debug.Log($"Updating current health: {currentHealth}, newsize: {newSize}, currentsize: {currentSize}, half: {isHalf}");

        if (currentSize > 0) SetMode(healthObjects[currentSize - 1], "full", false);

        // add items if increase
        if (currentSize < newSize)
        {
            for (int i = currentSize; i < newSize; i++)
            {
                GameObject newObj = healthObjects[i];
                SetMode(newObj, "full", false);
            }
        }
        // remove items if decreased
        else if (currentSize > newSize)
        {
            for (int i = currentSize; i > newSize; i--)
            {
                GameObject oldObj = healthObjects[i - 1];
                SetMode(oldObj, "empty", false);
            }
        }

        // set last heart to half if applicable
        if (isHalf)
        {
            // SetMode(healthObjects.Where((x) => x.transform.GetChild(1).gameObject.activeSelf).LastOrDefault(), "half", false);
            SetMode(healthObjects[newSize - 1], "half", false);
        }
    }

    private void SetMode(GameObject healthObj, string setTo, bool setBase)
    {
        GameObject halfObj = healthObj.transform.GetChild(setBase ? 0 : 1).gameObject;
        GameObject fullObj = halfObj.transform.GetChild(0).gameObject;

        switch (setTo)
        {
            case "empty":
                halfObj.SetActive(false);
                fullObj.SetActive(false);
                break;
            case "full":
                halfObj.SetActive(true);
                fullObj.SetActive(true);
                break;
            case "half":
                halfObj.SetActive(true);
                fullObj.SetActive(false);
                break;
            default:
                break;
        }
    }
}