using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

	[Serializable]
	public struct NamedImages {
		public string name;
		public Sprite namedSprite;
	}
	public Transform rootCanvas;

	public Transform prefabMiniMap;

	Transform [,] MiniMapSprites = new Transform[30,20];

	public BoardManager boardManager;

	public NamedImages[] images;
	Dictionary<string,Sprite> spritesDictionary = new Dictionary<string,Sprite>();
	
	void Awake () {
		//generate dictionary from array
		foreach(NamedImages element in images) {
			spritesDictionary.Add(element.name,element.namedSprite);
		}
		
	}

	// Use this for initialization
	void Start () {
		rootCanvas.GetComponent<Canvas>().enabled = false;
		Prepare();
	}

	void Prepare () {
		int x,y;
		for (y=0;y<boardManager.rows;y++) {
			for (x=0;x<boardManager.cols;x++) {
				RectTransform tmpSpr = Instantiate (prefabMiniMap) as RectTransform;
				tmpSpr.SetParent(rootCanvas);
				tmpSpr.transform.localScale = new Vector3(1,1,1);
				tmpSpr.transform.localPosition = new Vector3(x*8-116,y*8-68,0);
				MiniMapSprites[x,y] = tmpSpr;
			}
		}
	}

	public void Show() {
		
		int x,y;
		string tag;
		
		for (y=0;y<boardManager.rows;y++) {
			for (x=0;x<boardManager.cols;x++) {
				tag = boardManager.getTagXY(x,y);
				if (tag!=null) {
					MiniMapSprites[x,y].GetComponent<Image>().sprite = spritesDictionary[tag];
				}
				else {
					MiniMapSprites[x,y].GetComponent<Image>().sprite = spritesDictionary["empty"];
				}
			}
		}
		rootCanvas.GetComponent<Canvas>().enabled = true;
	}
	
	public void Hide() {
		rootCanvas.GetComponent<Canvas>().enabled = false;
	}
}
