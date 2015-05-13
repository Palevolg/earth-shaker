using UnityEngine;
using System.Collections;

public class InitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float delay = 2;
		Invoke ("LoadMenu", delay);

	}
	
	void LoadMenu() {
		Application.LoadLevel ("menu");
	}

}
