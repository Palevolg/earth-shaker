using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

	public bool falling;

	// Use this for initialization
	void Start () {
		falling = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool GetAttr() {
		return falling;
	}


	public void SetAttr(bool attr) {
		falling = attr;
	}
}
