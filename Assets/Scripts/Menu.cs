using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private ProfileManager profile = ProfileManager.GetInstance();

	// Use this for initialization
	void Start () {
		profile.Load ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Alpha1)) {
			GoToLevelSelector();
		}

		if (Input.GetKey(KeyCode.Alpha2)) {
			GoToOptions();
		}

		if (Input.GetKey(KeyCode.Alpha3)) {
			Quit ();
		}

	}


	public void GoToLevelSelector() {
		Application.LoadLevel ("levelSelector");
	}

	public void Quit() {
		Application.Quit ();
	}

	public void GoToOptions () {
		Application.LoadLevel("keysRedefine");
	}


}
