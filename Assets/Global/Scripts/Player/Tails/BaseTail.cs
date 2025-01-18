using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BaseTail", menuName = "BaseTail")]
public class BaseTail : ScriptableObject
{
    public string Name;
    public List<Attack> groundCombo;
    public List<Attack> airCombo;

    [HideInInspector] public List<Attack> currentCombo;

    public BaseTail(string name, List<Attack> GroundCombo, List<Attack> AirCombo)
    {
        Name = name;
        groundCombo = GroundCombo;
        airCombo = AirCombo;
    }
}
