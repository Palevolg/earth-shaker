using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;

public class BoardManager : MonoBehaviour {

	public int rows = 20;
	public int cols = 30;
	public int lvl;
	public GameObject player;

	private Transform boardHolder;

	private GameObject [,] levelMap = new GameObject[30,20];

	private string text = System.IO.File.ReadAllText("Assets/resources/levels.json");

	private object tempObj;


	void BoardSetup (int level)
	{	
		level = GameData.level - 1;

		//level = 1;

		boardHolder = new GameObject ("Board").transform;

		var N = JSON.Parse(text);
		int a, x, y;
		string doorSprite = "Door_"+N["levels"][level]["itemSprites"]["door"];
		string boulderSprite = "Boulder_"+N["levels"][level]["itemSprites"]["boulder"];
		string wallSprite = "Wall_"+N["levels"][level]["itemSprites"]["wall"];
		string earthSprite = "Earth_"+N["levels"][level]["itemSprites"]["earth"];

		//Debug.Log ("LEVEL "+(level+1)+": "+N["levels"][level]["title"]);
		//Debug.Log (doorSprite+"; "+boulderSprite+"; "+wallSprite+"; "+earthSprite+".");

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
						levelMap[x,y] = Instantiate (Resources.Load(doorSprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "2": {
						// boulder
						levelMap[x,y] = Instantiate (Resources.Load(boulderSprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "3": {/* player */}break;
					case "4": {
						
					}break;
					case "5": {
						// wall
					levelMap[x,y] = Instantiate (Resources.Load(wallSprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "6": {
						// earth
					levelMap[x,y] = Instantiate (Resources.Load(earthSprite), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "7": {
						// diamond
						levelMap[x,y] = Instantiate (Resources.Load("Diamond"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "8": {
						// jellyBean
						levelMap[x,y] = Instantiate (Resources.Load("JellyBean"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "9": {
						// forceField
						levelMap[x,y] = Instantiate (Resources.Load("ForceField"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "a": {
						// gravity
						levelMap[x,y] = Instantiate (Resources.Load("Gravity"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "b": {
						// forceField trigger
						levelMap[x,y] = Instantiate (Resources.Load("Trigger"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "c": {
						// elixir
						levelMap[x,y] = Instantiate (Resources.Load("Elixir"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "d": {
						// teleport
						levelMap[x,y] = Instantiate (Resources.Load("Teleport"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "e": {
						// bubble
						levelMap[x,y] = Instantiate (Resources.Load("Bubble"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
					case "f": {
						// fire
						levelMap[x,y] = Instantiate (Resources.Load("Fire"), new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					}break;
				}
				if (levelMap[x,y]!=null) levelMap[x,y].transform.SetParent (boardHolder);
			}
		} // for
		x = N["levels"][level]["player"]["x"].AsInt - 1;
		y = 20 - N ["levels"] [level] ["player"] ["y"].AsInt;
		player.transform.position = new Vector3(x,y,0);
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
