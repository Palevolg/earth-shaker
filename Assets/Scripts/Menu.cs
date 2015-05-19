using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
