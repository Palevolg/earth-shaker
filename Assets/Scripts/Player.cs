using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject player;
	public BoardManager boardManager;
	public CameraManager cameraManager;
	public Canvas infoScreen;
	public Canvas barUI;
	public MiniMap miniMap;
	Vector2 pos;

	public GameObject PrefabSFX;
	public GameObject SFXManagerInstance;
	SFXManager SFX;

	float checkTime;
	float lastTime;
	float tact;
	int timesToShowMinimap = 1;

	Vector2 primMove,secMove;
	char primKey, secKey;

	bool keyReleasedR,keyReleasedL,keyReleasedU,keyReleasedD, actionButton;

	GameManager GameData = GameManager.GetInstance();
	ProfileManager profile = ProfileManager.GetInstance();
	LoadedResources resources;

	void Awake () {
		SFX = SFXManagerInstance.GetComponent<SFXManager>();
	}

	public void TimerStop() {
		tact = 0f;
	}

	public void TimerRelease() {
		tact = GameData.tact;
	}

	// Use this for initialization
	void Start () {
		resources = GameObject.Find ("LoadedResources").GetComponent<LoadedResources>();
		tact = GameData.tact;
		float startDelay = 2f; //todo remove fixed delay

		lastTime = Time.time;
		checkTime = lastTime + startDelay;

		keyReleasedR = false;
		keyReleasedL = false;
		keyReleasedU = false;
		keyReleasedD = false;

		primKey = ' '; secKey = ' ';

		Invoke("RemoveInfo",startDelay);

	}

	void RemoveInfo() {
		infoScreen.enabled = false;
		barUI.enabled = true;
	}

	void LastKeyUpdate (char key) {
		secKey = primKey;
		primKey = key;
	}

	void LastKeyRemove (char key) {
		if (primKey == key) {
			primKey = secKey;
			secKey = ' ';
		}
		if (secKey == key) {
			secKey = ' ';
		}
	}

	// Update is called once per frame
	void Update () {

		lastTime = Time.time;
		if (lastTime >= checkTime && tact>Mathf.Epsilon) {
			UpdatePerTact();
			checkTime = Time.time + tact;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {LastKeyUpdate ('R');}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {LastKeyUpdate ('L');}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {LastKeyUpdate ('U');}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {LastKeyUpdate ('D');}

		if (Input.GetKeyUp (KeyCode.RightArrow)) {keyReleasedR = true; LastKeyRemove ('R');}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {keyReleasedL = true; LastKeyRemove ('L');}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {keyReleasedU = true; LastKeyRemove ('U');}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {keyReleasedD = true; LastKeyRemove ('D');}

		if (Input.GetKey (KeyCode.Q)) {SceneManager.LoadScene("levelSelector");}
	}

	void HideMiniMapInvoker() {
		miniMap.Hide();
		TimerRelease();
	}

	bool TryToMove(Vector2 moveVector) {
		bool result;
		result = false;
		pos.x += moveVector.x;
		pos.y += moveVector.y;
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
					result = true;
				}
				else {
					pos = transform.position;
				}
			}
			break;
		case "boulder":{
				int A = X+(int)moveVector.x;
				int B = Y;//+(int)moveAttempt.y;
				if (!boardManager.PushAsBoulder(X,Y,A,B)) {
					pos = transform.position;
				}
				else {
					SFX.PlaySFX("push");
					result = true;
				}
			}
			break;
		case "jellybean":{
				boardManager.destroyXY(X,Y);
				EnergyRefill();
				result = true;
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
				result = true;
			}
			break;
		case "elixir": {
				SFX.PlaySFX("elixir");
				boardManager.destroyXY(X,Y);
				GameData.lives++;
				result = true;
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
				result = true;
			}
			break;
		case "earth": {
				SFX.PlaySFX("earth");
				boardManager.destroyXY(X,Y);
				result = true;
			}
			break;
		case "wethellsoil": {
				boardManager.destroyXY(X,Y);
				result = true;
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
				result = true;
			}
			break;
		case "wall":{
				pos = transform.position;
			}
			break;
		case "bubble":{
				int A = X+(int)moveVector.x;
				int B = Y+(int)moveVector.y;
				if (!boardManager.PushAsBubble (X, Y, A, B)) { //can we push the bubble from XY to AB ?
					pos = transform.position;  //if can't - don't move
				} else {
					result = true;
				}
			}
			break;
		case "fire":{
				boardManager.destroyXY(X,Y);
				Die();
				result = true;
			}
			break;
		}

		if (actionButton) {pos=transform.position;}
		transform.position = pos;
		cameraManager.FollowPlayer();

		return result;
	}

	Vector2 DefineMoveByKey(char key, bool isPrim = false) {
		Vector2 move;
		move.x = 0;
		move.y = 0;

		switch (key) {
		case 'R':
			move.x = 1;
			break;
		case 'L':
			move.x = -1;
			break;
		case 'U':
			move.y = 1;
			break;
		case 'D':
			move.y = -1;
			break;
		}

		if ((isPrim) && (move.x == 0) && (move.y == 0)) {
			if ((key == 'R') && (keyReleasedR)) {
				move.x = 1;
			}
			if ((key == 'L') && (keyReleasedL)) {
				move.x = -1;
			}
			if ((key == 'U') && (keyReleasedU)) {
				move.y = 1;
			}
			if ((key == 'D') && (keyReleasedD)) {
				move.y = -1;
			}
		}

		return move;
	}

	void UpdatePerTact () {

		if (Input.GetKey (KeyCode.S)) {
			TimerStop();
			Die();
		}

		if (Input.GetKey (KeyCode.F)) {
			if (timesToShowMinimap-->0) {
				TimerStop();
				miniMap.Show();
				SFX.PlaySFX("minimap");
				Invoke("HideMiniMapInvoker",SFX.ClipDuration("minimap",1f));
			}
		}

		pos = transform.position;

		actionButton = Input.GetKey(KeyCode.Space);

		primMove = DefineMoveByKey (primKey, true);
		secMove = DefineMoveByKey (secKey);


		if (!TryToMove (primMove)) TryToMove(secMove);
			//try to move

		if (--GameData.energy<0) {
			Die();
		}

		boardManager.EnergyBarDraw(GameData.energy);

		boardManager.ProcessMap (); //check falling objects, melting boulders, triggers etc.
		boardManager.SetLevelText(GameData.level.ToString("D2"));
		boardManager.SetDiamondsText(GameData.diamondRequired.ToString("D2"), GameData.diamondsCollected.ToString("D2"));
		boardManager.SetLivesCountText(GameData.lives.ToString("D2"));
		boardManager.SetGravityTimer(GameData.gravityTimer.ToString("D2"));
		boardManager.SetScoreText(GameData.score.ToString("D6"));
	}

	void InvokerGamePlay() {SceneManager.LoadScene("gameplay");}
	void InvokerLevelSelector(){SceneManager.LoadScene("levelSelector");}

	void FinishLevel() {
		GetComponent<SpriteRenderer>().enabled=false; //hide player
		Debug.Log ("Level finished!");
		GameData.score+=GameData.energy;
		GameData.level++;
		tact = .0f; //stop updates;
		if (GameData.level>profile.LevelReached()) {
			profile.SaveLevelReached(GameData.level);
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