using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject player;
	public BoardManager boardManager;
	public CameraManager cameraManager;
	public Canvas infoScreen;
	public Canvas barUI;
	Vector2 pos;

	public GameObject PrefabSFX;
	public GameObject SFXManagerInstance;
	SFXManager SFX;

	float checkTime;
	float lastTime;
	float tact;

	Vector2 moveAttempt;

	bool keyPressedR, keyReleasedR,keyPressedL, keyReleasedL,keyPressedU, keyReleasedU,keyPressedD, keyReleasedD, actionButton;

	GameManager GameData = GameManager.GetInstance();
	ProfileManager profile = ProfileManager.GetInstance();
	LoadedResources resources;

	void Awake () {
		SFX = SFXManagerInstance.GetComponent<SFXManager>();
	}

	// Use this for initialization
	void Start () {
		resources = GameObject.Find ("LoadedResources").GetComponent<LoadedResources>();
		tact = GameData.tact;
		float startDelay = 2f; //todo remove fixed delay

		lastTime = Time.time;
		checkTime = lastTime + startDelay;

		moveAttempt = new Vector2 (0f, 0f);

		keyPressedR = false;keyReleasedR = false;
		keyPressedL = false;keyReleasedL = false;
		keyPressedU = false;keyReleasedU = false;
		keyPressedD = false;keyReleasedD = false;

		Invoke("RemoveInfo",startDelay);

	}

	void RemoveInfo() {
		infoScreen.enabled = false;
		barUI.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

		lastTime = Time.time;
		if (lastTime >= checkTime && tact>Mathf.Epsilon) {
			UpdatePerTact();
			checkTime = Time.time + tact;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {keyPressedR = true;}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {keyPressedL = true;}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {keyPressedU = true;}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {keyPressedD = true;}

		if (Input.GetKeyUp (KeyCode.RightArrow)) {keyReleasedR = true;}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {keyReleasedL = true;}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {keyReleasedU = true;}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {keyReleasedD = true;}

		if (Input.GetKey (KeyCode.Q)) {Application.LoadLevel("levelSelector");}
	}

	void UpdatePerTact () {

		if (Input.GetKey (KeyCode.S)) {
			tact = 0f;
			Die();
		}
		pos = transform.position;

		moveAttempt.x = 0;
		moveAttempt.y = 0;

		actionButton = Input.GetKey(KeyCode.Space);

		if (Input.GetKey (KeyCode.RightArrow)) {
			moveAttempt.x = 1;
		} else if (keyPressedR && keyReleasedR) {
			moveAttempt.x = 1;
		}
		keyPressedR = false;keyReleasedR = false;

		if (Input.GetKey (KeyCode.LeftArrow)) {
			moveAttempt.x = -1;
		} else if (keyPressedL && keyReleasedL) {
			moveAttempt.x = -1;
		}
		keyPressedL = false;keyReleasedL = false;

		if (Input.GetKey (KeyCode.UpArrow)) {
			moveAttempt.y = 1;
		} else if (keyPressedU && keyReleasedU) {
			moveAttempt.y = 1;
		}
		keyPressedU = false;keyReleasedU = false;

		if (Input.GetKey (KeyCode.DownArrow)) {
			moveAttempt.y = -1;
		} else if (keyPressedD && keyReleasedD) {
			moveAttempt.y = -1;
		}
		keyPressedD = false;keyReleasedD = false;

		if (moveAttempt.x != 0) {
			moveAttempt.y = 0;
		}

		if (moveAttempt.x != 0 || moveAttempt.y != 0) {
			//try to move
			pos.x += moveAttempt.x;
			pos.y += moveAttempt.y;
			if (pos.x < 0) {
				pos.x = 0;
			}
			if (pos.x>29) {
				pos.x = 29;
			}
			if (pos.y < 0) {
				pos.y=0;
			}
			if (pos.y>19) {
				pos.y = 19;
			}

			int X = (int) Mathf.Round(pos.x);
			int Y = (int) Mathf.Round(pos.y);

			string otherTag = boardManager.getTagXY (X,Y);
			if (resources.GetPropByTag(otherTag,"pushable")=="no" && actionButton) {
				// don't push unpushable
				pos=transform.position;
				otherTag=null;
			}

			switch (otherTag) {
			case "door":{
				if (boardManager.GetAttrXY(X,Y)){
					FinishLevel();
				}
				else {
				pos = transform.position;
				}
			}
			break;
			case "boulder":{
				int A = X+(int)moveAttempt.x;
				int B = Y;//+(int)moveAttempt.y;
				if (!boardManager.PushAsBoulder(X,Y,A,B)) {
					pos = transform.position;
				}
				else {
					SFX.PlaySFX("push");
				}
			}
			break;
			case "jellybean":{
				boardManager.destroyXY(X,Y);
				EnergyRefill();
			}
			break;
			case "forcefield":{
				pos = transform.position;
				break;
			}
			case "trigger":{
				pos = transform.position;
			}
			break;
			case "gravity": {
				boardManager.destroyXY(X,Y);
				SFX.PlaySFX("gravity");
				if (boardManager.gravityOff == false)
				{
					boardManager.setGravityImageDirection(180f);
				}
				boardManager.GravityOff();
				Debug.ClearDeveloperConsole();
				Debug.Log ("Gravity Taken");
			}
			break;
			case "elixir": {
				SFX.PlaySFX("elixir");
				boardManager.destroyXY(X,Y);
				GameData.lives++;
				Debug.Log("Elexir taken. Lives: "+GameData.lives);
			}
			break;
			case "teleport": {
				SFX.PlaySFX("teleport");
				boardManager.destroyXY(X,Y);  //destroy current teleport
				Vector2 newPos = boardManager.GetTeleport(); //get coordinates of new teleport
				X = (int) newPos.x;
				Y = (int) newPos.y;
				boardManager.destroyXY(X,Y);  //destroy new teleport
				pos.x = newPos.x;  
				pos.y = newPos.y;
			}
			break;
			case "earth": {
				SFX.PlaySFX("earth");
				boardManager.destroyXY(X,Y);
			}
			break;
			case "wethellsoil": {
				boardManager.destroyXY(X,Y);
			}
			break;
			case "diamond":{
				boardManager.destroyXY(X,Y);
				SFX.PlaySFX("diamond");
				GameData.diamondsCollected++;
				GameData.score+=GameData.pointsPerDiamond;
				if (GameData.diamondsCollected >= GameData.diamondRequired) {
					boardManager.DoorActivate();
				}
			}
			break;
			case "wall":{
				pos = transform.position;
			}
			break;
			case "bubble":{
				int A = X+(int)moveAttempt.x;
				int B = Y+(int)moveAttempt.y;
				if (!boardManager.PushAsBubble(X,Y,A,B)) { //can we push the bubble from XY to AB ?
					pos = transform.position;  //if can't - don't move
				}
			}
			break;
			case "fire":{
				boardManager.destroyXY(X,Y);
				Die();
			}
			break;
			}

			if (actionButton) {pos=transform.position;}
			transform.position = pos;
			cameraManager.FollowPlayer();

		}

		if (--GameData.energy<0) {
			Die();
		}

		boardManager.EnergyBarDraw(GameData.energy);

		boardManager.ProcessMap (); //check falling objects, melting boulders, triggers etc.
		boardManager.SetLevelText(GameData.level.ToString("D2"));
		boardManager.SetDiamondsText(GameData.diamondRequired.ToString("D2"), GameData.diamondsCollected.ToString("D2"));
		boardManager.SetLivesCountText(GameData.lives.ToString("D2"));
		boardManager.SetGravityTimer(GameData.gravityTimer.ToString("D2"));
		boardManager.SetDebugText("  Energy: "+GameData.energy.ToString("D4"));
	}

	void InvokerGamePlay() {Application.LoadLevel("gameplay");}
	void InvokerLevelSelector(){Application.LoadLevel("levelSelector");}

	void FinishLevel() {
		GetComponent<SpriteRenderer>().enabled=false; //hide player
		Debug.Log ("Level finished!");
		GameData.score+=GameData.energy;
		GameData.level++;
		tact = .0f; //stop updates;
		PlayerPrefs.SetInt ("LevelReached", GameData.level);
		if (GameData.level>profile.levelReached) {
			profile.levelReached = GameData.level;
		}
		SFX.PlaySFX("finish");
		Invoke("InvokerGamePlay", SFX.ClipDuration("finish",1f));
	}

	public void Die() {
		GetComponent<Animator>().enabled = false;
		SFX.PlaySFX("death");
		tact = .0f; //stop updates;
		//todo death sprite change or animation
		if (--GameData.lives>0) {
			Invoke("InvokerGamePlay", SFX.ClipDuration("death",1f));
		}
		else {
			Debug.Log("GAME OVER");
			Invoke("InvokerLevelSelector", SFX.ClipDuration("death",1f)); //todo invoke gameover
		}
	}

	public void EnergyRefill() {
		SFX.PlaySFX("energy",.6f); //очень уж противный звук... потише
		GameData.energy+= resources.energyPart;
		if (GameData.energy > resources.energyFull) GameData.energy = resources.energyFull;
	}
}