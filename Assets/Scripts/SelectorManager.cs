using UnityEngine;
using System.Collections;

public class SelectorManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel_1 () {
		GameData.level = 1;
		Application.LoadLevel ("gameplay");
	}

	public void loadLevel_2 () {
		GameData.level = 2;
		Application.LoadLevel ("gameplay");
	}
}
