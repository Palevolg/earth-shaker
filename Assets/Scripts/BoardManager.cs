using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

	public int rows = 20;
	public int cols = 30;
	public int lvl;
	public GameObject player;
	public CameraManager cameraManager;
	public GameObject SFXManagerInstance;
	SFXManager SFX;

	public Text InfoLevel; 
	public Text InfoTitle;
	public Text InfoLives;
	public Text InfoScore;

	public Image energyWrapper;
	public Text debugText;
	public Text LevelNumberText;
	public Text diamondsNeededText;
	public Text diamondsCollectedText;
	public Text livesCountText;
	public Text gravityTimerText;
	public Image gravityDirection;
	public bool gravityOff = false;

	LoadedResources resources;
	private Transform boardHolder;

	private GameObject [,] levelMap = new GameObject[30,20];

	private object tempObj;

	//ResourceManager resources = ResourceManager.GetInstance(); //get resources Singleton
	GameManager GameData = GameManager.GetInstance();

	void Awake () {
		SFX = SFXManagerInstance.GetComponent<SFXManager>();
	}

	private void PlaceItem(int x, int y, string sprite) {
		levelMap[x,y] = Instantiate (Resources.Load(sprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
	}

	public void SetLevelText(string str) {
		LevelNumberText.text = str;
	}
	public void SetDiamondsText(string needed, string collected) {
		diamondsNeededText.text = needed;
		diamondsCollectedText.text = collected;
	}
	public void SetLivesCountText(string str) {
		livesCountText.text = str;
	}
	public void setGravityImageDirection(float direction)
	{	
		gravityDirection.rectTransform.Rotate(direction,0f,0f);
	}
	public void SetGravityTimer(string str) {
		gravityTimerText.text = str;
	}
	
	public void SetDebugText(string str) {
		debugText.text = str;
	}

	void BoardSetup ()
	{	
		int level = GameData.level - 1;

		if (GameData.level==0) level = 0; //todo remove;

		GameData.diamondPlaced = 0;
		GameData.diamondsCollected = 0;
		GameData.gravityTimer = 0;
		GameData.energy = resources.energyFull;

		GameData.diamondRequired = resources.GetLevelSettings(level,"diamondsRequired");
		GameData.pointsPerDiamond = resources.GetLevelSettings(level,"pointsPerDiamond");

		boardHolder = new GameObject ("Board").transform;

		int a, x, y;
		string doorSprite = resources.GetSpriteTitle(level,"door");
		string boulderSprite = resources.GetSpriteTitle(level,"boulder");
		string wallSprite = resources.GetSpriteTitle(level,"wall");
		string earthSprite = resources.GetSpriteTitle(level,"earth");

		for(a = 0; a < rows; a++)	{
			y = rows-a-1;
			for(x = 0; x < cols; x++)	{	
				switch (resources.GetLevelItemXY(level,x,a)){
					case "0": {
						// empty
						levelMap[x,y] = null;
					}break;
					case "1": {
						// door
						PlaceItem(x,y,doorSprite);
					}break;
					case "2": {
						// boulder
						PlaceItem(x,y,boulderSprite);
					}break;
					case "3": {levelMap[x,y] = null;/* player */}break;
					case "4": {levelMap[x,y] = null;			}break;
					case "5": {
						// wall
						PlaceItem(x,y,wallSprite);
					}break;
					case "6": {
						// earth
						PlaceItem(x,y,earthSprite);
					}break;
					case "7": {
						// diamond
						PlaceItem(x,y,"Diamond");
						GameData.diamondPlaced++;
					}break;
					case "8": {
						// jellyBean
						PlaceItem(x,y,"JellyBean");
					}break;
					case "9": {
						// forceField
						PlaceItem(x,y,"ForceField");
					}break;
					case "a": {
						// gravity
						PlaceItem(x,y,"Gravity");
					}break;
					case "b": {
						// forceField trigger
						PlaceItem(x,y,"Trigger");
					}break;
					case "c": {
						// elixir
						PlaceItem(x,y,"Elixir");
					}break;
					case "d": {
						// teleport
						PlaceItem(x,y,"Teleport");;
					}break;
					case "e": {
						// bubble
						PlaceItem(x,y,"Bubble");
					}break;
					case "f": {
						// fire
						PlaceItem(x,y,"Fire");
					}break;
				}
				if (levelMap[x,y]!=null) levelMap[x,y].transform.SetParent (boardHolder);
			}
		} // for
		player.transform.position = resources.GetPlayerCoords(level);
		cameraManager.FollowPlayer ();
	}

	public Vector2 GetTeleport() {
		int x, y;
		for (y = rows-1; y>=0; y--) {
			for (x=0; x<cols; x++) {
				if (getTagXY(x,y)=="teleport") {
					return new Vector2(x,y);
				}
			}
		}

		return new Vector2 (0, 0); //todo check if no teleports
	}

	public bool GetAttrXY(int x, int y) {
		return levelMap [x, y].GetComponent<ItemManager>().GetAttr();
	}

	public void SetAttrXY(int x, int y, bool attr) {
		levelMap [x, y].GetComponent<ItemManager> ().SetAttr (attr);
	}

	public void DoorActivate() {
		SFX.PlaySFX("doorActivated");
		GameObject door = GameObject.FindWithTag("door");
		door.GetComponent<ItemManager> ().SetAttr (true);
		door.GetComponent<Animator> ().enabled = true;
	}

	public string getTagXY(int x, int y) {
		if (x == player.transform.position.x && y == player.transform.position.y) {
			return "player";
		}
		if (levelMap [x, y] != null) {
			return levelMap [x, y].tag;
		} else {
			return null;
		}
	}

	public void destroyXY(int x, int y) {
		if (levelMap [x, y] != null)
			//DestroyImmediate (levelMap [x, y]);
			levelMap[x,y].tag = "toDestroy";
			Destroy (levelMap [x, y]);
	}

	private IEnumerator MeltBoulder(GameObject boulder){
		boulder.tag = "wall";
		boulder.GetComponent<Animator>().enabled = true;
		boulder.GetComponent<ItemManager>().sfxDestroingNoPlayer();
		yield return new WaitForSeconds (.6f); //todo get animation time
		Destroy (boulder);
	}
	private IEnumerator BubbleBlow(GameObject bubble){
		bubble.GetComponent<Animator> ().SetTrigger ("BubbleBlow");
		bubble.GetComponent<ItemManager>().sfxDestroingNoPlayer();
		yield return new WaitForSeconds (0.4f); //todo get animation time
		Destroy (bubble);
	}


	public void moveXYtoAB(int X, int Y, int A, int B) {
		//move object from XY to AB
		levelMap [A, B] = levelMap [X, Y];
		levelMap [X, Y] = null;
		if (levelMap [A, B] != null) {
			levelMap [A, B].transform.position = new Vector3 (A, B, 0f);
		}
	}

	public bool PushAsBubble(int X, int Y, int A, int B) {
		if (A < 0 || A > 29 || B < 0 || B > 19) {
			return false;
		}
		switch (getTagXY (A, B)) {
		case "earth":
			{	
			SFX.PlaySFX("earth");
				destroyXY (A, B);
				moveXYtoAB (X, Y, A, B);
				return true;
			}
		case "fire":
			{
				destroyXY (A, B);
				moveXYtoAB (X, Y, A, B);
				StartCoroutine(BubbleBlow(levelMap[A,B]));

			return true;
			}
		case null:
			{
				moveXYtoAB (X, Y, A, B);
			return true;
			}
		}
		return false;
	}

	public bool PushAsBoulder(int X, int Y, int A, int B) {
		if (A<0 || A>29) {
			return false;
		}
		if (getTagXY(A,B)==null){
			moveXYtoAB(X,Y,A,B);
			return true;
		}
		return false;
	}

	public void ProcessMap() {
		CheckFalls();
		if (GameData.gravityTimer>0) {
			GameData.gravityTimer--;
		}
		else {
			if (gravityOff == true) 
			{
				gravityOff = false;
				setGravityImageDirection(180f);
			}	
			CheckStatic ();
		}
	}

	public void GravityOff() {
		GameData.gravityTimer = 100;
		gravityOff = true;
		for (int y = 0; y<rows; y++){
			for (int x = 0; x<cols; x++) {
				if (resources.GetPropByTag(getTagXY(x,y), "gResponds")=="yes") {
					SetAttrXY(x,y,false);
				}
			}
		}

	}

	private void CheckFalls() {
		bool g = (GameData.gravityTimer>0); // true if no gravity
		int gravityGradient = (g)?-1:1;    // -1 - to fall up, +1 - down
		int x, y, b;
		string tag;
		for (y = g?18:1; (y<20 && y>=0); y+=gravityGradient) { 
			b = y-gravityGradient; 
			for (x=0; x<cols; x++) {
				tag = getTagXY(x,y);
				if (resources.GetPropByTag(tag,"gResponds")=="yes") {
					if (getTagXY(x,b)==null) {
						if (GetAttrXY(x,y)) {
							moveXYtoAB(x,y,x,b);
							SFX.PlaySFX("falling");
						}
						else {
							//falling start
							SetAttrXY(x,y,true);
						}
					}
					
					else if (resources.GetPropByTag(getTagXY(x,b),"fallOff")=="yes") {
						if (x>0 && getTagXY(x-1,b)==null && getTagXY(x-1,y)==null) {
							PushAsBoulder(x,y,x-1,y);
							SetAttrXY(x-1,y,true);
						}
						else if (x<29 && getTagXY(x+1,b)==null && getTagXY (x+1,y)==null) {
							PushAsBoulder(x,y,x+1,y);
							SetAttrXY(x+1,y,true);
							x++;
						}
						else {
							SetAttrXY(x,y,false);
						}
					}
					else {
						if (GetAttrXY(x,y)) {SFX.PlaySFX("fallen");}  //todo check if normal gravity
						SetAttrXY(x,y,false);
					}
				}
			}
		}
		y = g?19:0;
		for (x=0; x<cols; x++) { // set attribute to all items on ground — false
			tag=getTagXY(x,y);
			if (resources.GetPropByTag(tag,"gResponds")=="yes") {SetAttrXY(x,y,false);}
		}
	}

	void ForceFieldRemove() {
		int x, y;
		SFX.PlaySFX ("trigger");
		for (y = 0; y<rows; y++) { 
			for (x=0; x<cols; x++) {
				if (getTagXY(x,y)=="forcefield") {destroyXY(x,y);}
			}
		}
	}

	private void CheckStatic() {
		int x, y, b;
		string tag;
		for (y = 1; y<20; y++) { 
			b = y - 1; 
			for (x=0; x<30; x++) {
				tag = getTagXY(x,y);
				if (tag == "boulder" && getTagXY(x,b) == "fire" && !GetAttrXY(x,b)) {
					StartCoroutine(MeltBoulder(levelMap[x,y]));
				}
				if (getTagXY(x,b) == "trigger") {
					if (resources.GetPropByTag(tag,"gResponds")=="yes" && GetAttrXY(x,y)) {
						destroyXY(x,b);
						ForceFieldRemove();
					}
				}
				if (getTagXY(x,b) == "player") {
					if (resources.GetPropByTag(tag,"gResponds")=="yes" && GetAttrXY(x,y)) {
						player.GetComponent<Player>().Die();
					}
				}
			}
		}
	}

	public void EnergyBarDraw (float part) {
		energyWrapper.GetComponent<RectTransform>().sizeDelta = new Vector2(.256f*(1000-part), 6f);
	}

	void InfoSetup() {

		InfoLevel.text = "LEVEL "+GameData.level.ToString("D2");

		InfoTitle.text = resources.GetLevelTitle(GameData.level-1);

		InfoLives.text = "LIVES: "+GameData.lives;

		InfoScore.text = "SCORE: "+GameData.score.ToString("D6");
	}

	public void SetupScene () {
		BoardSetup ();
	}

	// Use this for initialization
	void Start () {
		resources = GameObject.Find ("LoadedResources").GetComponent<LoadedResources>();
		InfoSetup();
		SetupScene ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
