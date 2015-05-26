using UnityEngine;
using System.Collections;

public class GameManager {

	public int level;
	public int score;
	public int lives;
	public int gravityTimer;
	public float tact = 0.1f;
	public int diamondsCollected;
	public int diamondPlaced;
	public int diamondRequired;
	public int pointsPerDiamond;
	
	private static GameManager instance;
	
	public static GameManager GetInstance() {
		if (instance == null)
			instance = new GameManager ();
		return instance;
	}
}