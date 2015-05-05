using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public int rows = 20;
	public int cols = 30;

	private Transform boardHolder;

	private GameObject [,] levelMap = new GameObject[30,20];

	public GameObject earth;  //todo remove

	void BoardSetup ()
	{
		boardHolder = new GameObject ("Board").transform;
		
		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for(int y = 0; y < rows; y++)
		{
			//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			for(int x = 0; x < cols; x++)
			{
				if ((x*3+y*2)%7 == 0) {
					levelMap[x,y] = Instantiate (Resources.Load("BoulderYellow"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;}
				else {
					levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;}

				levelMap[x,y].transform.SetParent (boardHolder);
			}
		}
	}

	public void SetupScene (int level) {
		BoardSetup ();
	}

	// Use this for initialization
	void Start () {
		SetupScene (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
