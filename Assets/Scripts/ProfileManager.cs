using UnityEngine;
using System.Collections;

public class ProfileManager {
	
	string name;
	int levelReached;
	public float musicVolume;
	public float sfxVolume;

	private static ProfileManager instance;

	public static ProfileManager GetInstance() {
		if (instance == null) {
			instance = new ProfileManager ();
		}
		return instance;
	}

	public void SetName(string n) {
		name = n;
	}

	public string GetName() {
		return name;
	}

	public int LevelReached() {
		int lvl;
		lvl = PlayerPrefs.GetInt("LevelReached");
		if (lvl<1) {
			lvl = 1;
			Save();
		}
		return levelReached;
	}

	public void SaveLevelReached(int lvl) {
		levelReached = lvl;
		Save();
	}

	public void Load() {
		levelReached = (PlayerPrefs.HasKey ("LevelReached")) ? PlayerPrefs.GetInt ("LevelReached") : 1;
	}
	
	public void Save() {
		//todo save provile
		PlayerPrefs.SetInt ("LevelReached", levelReached);
	}

	public void Create() {
		//create profile
	}
}