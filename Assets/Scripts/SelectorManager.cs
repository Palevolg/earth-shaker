using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorManager : MonoBehaviour {

	public Slider levelSlider;
	public Button levelButton;

	GameManager GameData = GameManager.GetInstance();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel_1 () {
		LevelStart(1);
	}

	public void loadLevel_2 () {
		LevelStart(2);
	}

	public void goToLevel () {
		LevelStart(Mathf.RoundToInt(levelSlider.value));
	}

	public void ShowLevel () {
		Text temp = levelButton.GetComponentInChildren<Text> ();
		temp.text = Mathf.RoundToInt(levelSlider.value).ToString();
	}

	void LevelStart(int lvl) {
		GameData.lives = 5;
		GameData.score = 0;
		GameData.level = lvl;
		Application.LoadLevel ("gameplay");
	}
}
