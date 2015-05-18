using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject player;
	private Vector2 pos;

	private float checkTime;
	private float lastTime;

	private Vector2 moveAttempt;

	public void placeToXY (int x, int y) {

	}

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
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		//GroundScript ground = otherCollider.gameObject.GetComponent<GroundScript>();
		GameObject otherObject = otherCollider.gameObject;
		string tag = otherObject.tag;
		/*if (ground != null)
		{
			// Destroy the ground
			Destroy(ground.gameObject); 
			
		}*/
		switch (tag) {
		case "earth": {
			Destroy(otherObject);
			break;
		}
		case "diamond": {
			Destroy(otherObject);
			break;
		}
		}
	}
	void UpdatePerTact () {
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

		pos = transform.position;
		pos.x += moveAttempt.x;
		pos.y += moveAttempt.y;
		transform.position = pos;

		moveAttempt.x = 0;
		moveAttempt.y = 0;

		// check falling objects

	}
}
