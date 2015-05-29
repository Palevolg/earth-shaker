using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectorManager : MonoBehaviour {

	public Slider levelSlider;
	public Button levelButton;

	public Transform lvlBtn;
	public Transform UICanvas;

	private ProfileManager profile = ProfileManager.GetInstance();

	GameManager GameData = GameManager.GetInstance();
	// Use this for initialization
	void Start () {
		int x, y, a;
		for (int lvl=0; lvl<32; lvl++) {
			x = (lvl%8)*100-350;
			y = 100-Mathf.RoundToInt(lvl/8)*100;
			RectTransform tmpBtn = Instantiate(lvlBtn) as RectTransform;
			tmpBtn.SetParent(UICanvas);
			tmpBtn.transform.localScale = new Vector3(1,1,1);
			tmpBtn.transform.localPosition = new Vector3(x,y,0);

			a = lvl+1;
			tmpBtn.transform.FindChild("Text").GetComponent<Text>().text = a.ToString("D2");
			tmpBtn.GetComponent<GoToLevel>().level = a;
			tmpBtn.GetComponent<Button>().onClick.AddListener(() => { LevelStart(tmpBtn.GetComponent<GoToLevel>().level); });
			if (a>profile.levelReached) {
				tmpBtn.GetComponent<Button>().interactable = false;
			}
		}
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
