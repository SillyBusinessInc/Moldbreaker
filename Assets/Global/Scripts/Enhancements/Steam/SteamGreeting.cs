using UnityEngine;
using Steamworks;
using TMPro;

public class SteamScript : MonoBehaviour {
	void Start() {
		if (SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();

            GetComponent<TMP_Text>().text = $"Good to see you {name}";
		}
		else {
			GetComponent<TMP_Text>().text = $"Could not connect to steam client...";
		}
	}
}