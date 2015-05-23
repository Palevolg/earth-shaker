using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject player;
	public BoardManager boardManager;
	public CameraManager cameraManager;
	Vector2 pos;

	float checkTime;
	float lastTime;

	Vector2 moveAttempt;

	// Use this for initialization
	void Start () {

		lastTime = Time.time;
		checkTime = lastTime + GameData.tact;

		moveAttempt = new Vector2 (0f, 0f);
	
	}
	
	// Update is called once per frame
	void Update () {

		moveAttempt.x += Input.GetAxisRaw ("Horizontal");
		moveAttempt.y += Input.GetAxisRaw ("Vertical");

		lastTime = Time.time;
		if (lastTime >= checkTime) {
			UpdatePerTact();
			checkTime = Time.time + GameData.tact;
		}
		

	}

	void UpdatePerTact () {
		pos = transform.position;
		// player move
		if (moveAttempt.x > 0) {
			moveAttempt.x = 1;
			moveAttempt.y =0;
		}
		if (moveAttempt.x < 0) {
			moveAttempt.x = -1;
			moveAttempt.y =0;
		}
		if (moveAttempt.y>0) {
			moveAttempt.y = 1;
		}
		if (moveAttempt.y<0) {
			moveAttempt.y = -1;
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
			}
			break;
			case "jellybean":{
				boardManager.destroyXY(X,Y);
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
				Debug.ClearDeveloperConsole();
				Debug.Log ("Gravity Taken");
			}
			break;
			case "elixir": {
				boardManager.destroyXY(X,Y);
			}
			break;
			case "teleport": {
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
				boardManager.destroyXY(X,Y);
			}
			break;
			case "wethellsoil": {
				boardManager.destroyXY(X,Y);
			}
			break;
			case "diamond":{
				boardManager.destroyXY(X,Y);
				GameData.diamondsCollected++;
				if (GameData.diamondsCollected >= GameData.diamondPlaced) {
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
				//todo death
			}
			break;
			}
			if (Input.GetKey(KeyCode.Space)) {pos=transform.position;}
			transform.position = pos;
			cameraManager.FollowPlayer();

			moveAttempt.x = 0;
			moveAttempt.y = 0;

		} 

		boardManager.ProcessMap (); //check falling objects, melting boulders, triggers etc.
	}

	void FinishLevel() {
		Debug.Log ("Level finished!");
		GameData.level++;
		Application.LoadLevel("gameplay");
	}

}