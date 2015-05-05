using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;

public class BoardManager : MonoBehaviour {

	public int rows = 20;
	public int cols = 30;

	private Transform boardHolder;

	private GameObject [,] levelMap = new GameObject[30,20];

	private string text = System.IO.File.ReadAllText("Assets/resources/levels.json");

	private object tempObj;


	void BoardSetup (int level)
	{
		boardHolder = new GameObject ("Board").transform;

		var N = JSON.Parse(text);

		for(int y = 0; y < rows; y++)	{
			for(int x = 0; x < cols; x++)	{	
				switch (N["levels"][level]["map"][y][x]){
					case "0": {
						// empty
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "1": {
						// door
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "2": {
						// boulder
						levelMap[x,y] = Instantiate (Resources.Load("BoulderYellow"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "3": {
						// player
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "4": {
						// player
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "5": {
						// wall
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "6": {
						// earth
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "7": {
						// diamond
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "8": {
						// jellyBean
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "9": {
						// forceField
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "a": {
						// gravity
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "b": {
						// forceField trigger
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "c": {
						// elixir
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "d": {
						// teleport
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "e": {
						// bubble
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "f": {
						// fire
						levelMap[x,y] = Instantiate (Resources.Load("EarthBlue"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
				}
				levelMap[x,y].transform.SetParent (boardHolder);
			}
		}
	}

	public void SetupScene (int level) {
		BoardSetup (level);
	}

	// Use this for initialization
	void Start () {
		SetupScene (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
