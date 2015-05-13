using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorManager : MonoBehaviour {

	public Slider levelSlider;
	public Button levelButton;

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

	public void goToLevel () {
		GameData.level = Mathf.RoundToInt(levelSlider.value);
		Application.LoadLevel ("gameplay");
	}

	public void ShowLevel () {
		Text temp = levelButton.GetComponentInChildren<Text> ();
		temp.text = Mathf.RoundToInt(levelSlider.value).ToString();
	}
}
