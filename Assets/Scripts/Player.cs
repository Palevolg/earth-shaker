using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject player;
	private Vector2 pos;
	public void placeToXY (int x, int y) {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pos = transform.position;
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			//RaycastHit2D hit;
			//Vector2 start = pos;
			//Vector2 end = start + new Vector2(pos.x + 1, pos.y);
			//hit = Physics2D.Linecast(start, end);
			//Debug.Log(hit.collider);
			//if (hit.transform == null){ 
				pos.x += 1;
				transform.position = pos;
			//}
			
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			pos.x -= 1;
			transform.position = pos; 		
			
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			pos.y += 1;
			transform.position = pos; 		
			
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			pos.y -= 1;
			transform.position = pos; 		
			
		}
		

	}
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		GroundScript ground = otherCollider.gameObject.GetComponent<GroundScript>();
		if (ground != null)
		{
			// Destroy the ground
			Destroy(ground.gameObject); 
			
		}
	}
}
