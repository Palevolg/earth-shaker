using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float delay = 2;
		Invoke ("LoadMenu", delay);

	}
	
	void LoadMenu() {
		SceneManager.LoadScene ("menu");
	}

}
