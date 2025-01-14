using System.Collections.Generic;
using UnityEngine;

public class MoldOrbManager : MonoBehaviour
{
    [SerializeField] private MoldOrb[] orbList;
    [SerializeField] public GameObject[] MoldObjects ;
    
    void Start()
    {
        orbList = FindObjectsByType<MoldOrb>(FindObjectsSortMode.None);
    }

    public void RemoveFromList(MoldOrb Orb) {
        for (int i = 0 ; i < orbList.Length ; i++){
            if (orbList[i] == Orb) {
                orbList = RemoveAtUsingList(orbList, i);
                break;
            }
        }
    }

    static MoldOrb[] RemoveAtUsingList(MoldOrb[] source, int index){
        List<MoldOrb> list = new List<MoldOrb>(source);
        list.RemoveAt(index);
        return list.ToArray();
    }   

    public void DestroyDoor() {
        if (orbList.Length <= 0 && MoldObjects.Length > 0) {
            foreach (GameObject obj in MoldObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
