using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private ProfileManager profile = ProfileManager.GetInstance();

	// Use this for initialization
	void Start () {
		profile.levelReached = PlayerPrefs.GetInt ("LevelReached");
		if (profile.levelReached<1) profile.levelReached = 1; //todo change to normal method
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Alpha1)) {
			Invoke("GoToLevelSelector", 0.0f);
		}
		if (Input.GetKey(KeyCode.Alpha3)) {
			Application.Quit ();
		}

	}


	public void GoToLevelSelector() {
		Application.LoadLevel ("levelSelector");
	}


}
