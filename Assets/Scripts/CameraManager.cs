using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject cameraInstance;
	public GameObject player;

	private float x,y;

	public void FollowPlayer() {
		x = player.transform.position.x;
		y = player.transform.position.y;
		x -= .5f;
		y -= .5f;
		if (x < 7.5f) {x = 7.5f;}
		if (x > 21.5f) {x = 21.5f;}
		if (y < 4.5f) {y = 4.5f;}
		if (y > 13.5f) {y = 13.5f;};
		cameraInstance.transform.position = new Vector3 (x, y, -10f);

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		FollowPlayer (); //todo: Optimize. call only with moves, teleports, etc.
	}
}
