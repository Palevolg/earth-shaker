using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LoadedResources : MonoBehaviour {

	string text;
	JSONNode N;
	public int energyPart = 125;
	public int energyFull = 1000;
	public TextAsset txtFile;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		//TextAsset txtFile = Resources.Load ("levels.json") as TextAsset;
		N = JSON.Parse(txtFile.text);
	}

	private string Capitalize (string str) {
		return char.ToUpper(str[0]) + str.Substring(1);
	}
	
	public int GetLevelSettings(int level, string option) {
		return N["levels"][level]["settings"][option].AsInt;
	}
	
	public string GetSpriteTitle(int level, string name) {
		return Capitalize(name)+"_"+N["levels"][level]["itemSprites"][name];
	}
	
	public string GetLevelItemXY(int level, int x, int y) {
		return N["levels"][level]["map"][y][x];
	}
	
	public Vector3 GetPlayerCoords(int level) {
		return new Vector3(N["levels"][level]["player"]["x"].AsInt - 1, 20 - N ["levels"] [level] ["player"] ["y"].AsInt, 0);
	}
	
	public string GetPropByTag(string tag, string prop) {
		string buf;
		if (tag != null) {
			buf = N ["itemsprop"][tag][prop];
		} else
			buf = null;
		return buf;
	}
	
	public string GetLevelTitle(int level) {
		return N["levels"][level]["title"];
	}
}
