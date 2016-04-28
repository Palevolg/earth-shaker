using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeysManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			BackToMenu ();
		}
	}

	public void BackToMenu () {
		SceneManager.LoadScene ("menu");
	}
}
