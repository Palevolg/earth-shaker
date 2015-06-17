using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {

	GameObject [,] MiniMapSprites = new GameObject[30,20];

	public BoardManager boardManager;

	// Use this for initialization
	void Start () {
		int x,y;
		for (y=0;y<boardManager.rows;y++) {
			for (x=0;x<boardManager.cols;x++) {
				//MiniMap[x,y].transform.SetParent(MiniMapCanvas.transform);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show() {
		
		int x,y;
		string tag;
		
		//if (--timesToShowMinimap<0) return;
		
		//TimerStop();
		//todo prepare minimap
		for (y=0;y<boardManager.rows;y++) {
			for (x=0;x<boardManager.cols;x++) {
				tag = boardManager.getTagXY(x,y);
				switch (tag) {
					
				}
			}
		}
		
		//MiniMapCanvas.enabled = true;
		//SFX.PlaySFX("minimap");
		//Invoke("MiniMapRemove",SFX.ClipDuration("minimap",1f));
		
	}
	
	void Remove() {
		//MiniMapCanvas.enabled = false;
		//TimerRelease();
	}
}
