using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;

public class BoardManager : MonoBehaviour {

	public int rows = 20;
	public int cols = 30;
	public int lvl;
	public GameObject player;
	public CameraManager cameraManager;

	private Transform boardHolder;

	private GameObject [,] levelMap = new GameObject[30,20];

	private string text;

	private object tempObj;

	private JSONNode N;

	private void PlaceItem(int x, int y, string sprite) {
		levelMap[x,y] = Instantiate (Resources.Load(sprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
	}

	void BoardSetup (int level)
	{	
		level = GameData.level - 1;

		if (GameData.level==0) level = 0; //todo remove;

		GameData.diamondPlaced = 0;
		GameData.diamondsCollected = 0;

		boardHolder = new GameObject ("Board").transform;

		int a, x, y;
		string doorSprite = "Door_"+N["levels"][level]["itemSprites"]["door"];
		string boulderSprite = "Boulder_"+N["levels"][level]["itemSprites"]["boulder"];
		string wallSprite = "Wall_"+N["levels"][level]["itemSprites"]["wall"];
		string earthSprite = "Earth_"+N["levels"][level]["itemSprites"]["earth"];

		for(a = 0; a < rows; a++)	{
			y = rows-a-1;
			for(x = 0; x < cols; x++)	{	
				switch (N["levels"][level]["map"][a][x]){
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
		x = N["levels"][level]["player"]["x"].AsInt - 1;
		y = 20 - N ["levels"] [level] ["player"] ["y"].AsInt;
		player.transform.position = new Vector3(x,y,0);
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

	public string getPropByTag(string tag, string prop) {
		string buf;
		if (tag != null) {
			buf = N ["itemsprop"][tag][prop];
		} else
			buf = null;
		return buf;
	}

	public void DoorActivate() {
		Debug.ClearDeveloperConsole ();
		Debug.Log ("Door activated");
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
		boulder.GetComponent<Animator>().enabled = true;
		yield return new WaitForSeconds (.6f); //todo get animation time
		Destroy (boulder);
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
				destroyXY (A, B);
				moveXYtoAB (X, Y, A, B);
				return true;
			}
		case "fire":
			{
				destroyXY (A, B);
				destroyXY (X, Y);
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
		CheckStatic ();
		CheckFalls ();
	}

	private void CheckFalls() {
		int x, y, b;
		string tag;
		for (y = 1; y<20; y++) { 
			b = y-1; 
			for (x=0; x<30; x++) {
				tag = getTagXY(x,y);
				if (getPropByTag(tag,"gResponds")=="yes") {
					
					if (getTagXY(x,b)==null) {
						if (GetAttrXY(x,y)) {
							moveXYtoAB(x,y,x,b);
						}
						else {
							SetAttrXY(x,y,true);
						}
					}
					
					else if (getPropByTag(getTagXY(x,b),"fallOff")=="yes") {
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
						SetAttrXY(x,y,false);
					}
				}
			}
		}
		for (x=0; x<cols; x++) { // set attribute to all items on ground — false
			tag=getTagXY(x,0);
			if (getPropByTag(tag,"gResponds")=="yes") {SetAttrXY(x,0,false);}
		}
	}
	private void ForceFieldRemove() {
		int x, y;
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
					if (getPropByTag(tag,"gResponds")=="yes" && GetAttrXY(x,y)) {
						destroyXY(x,b);
						ForceFieldRemove();
					}
				}
			}
		}
	}

	public void SetupScene (int level) {
		BoardSetup (level);
	}

	// Use this for initialization
	void Start () {
		text = System.IO.File.ReadAllText("Assets/resources/levels.json");
		N = JSON.Parse(text);
		SetupScene (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
