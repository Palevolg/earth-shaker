using UnityEngine;
using System.Collections;

public class ProfileManager {
	
	string name;
	
	private static ProfileManager instance;
	
	public static ProfileManager GetInstance() {
		if (instance == null)
			instance = new ProfileManager ();
		return instance;
	}

	public void SetName(string n) {
		name = n;
	}

	public string GetName() {
		return name;
	}

	public void Load() {
		//todo load profile
	}
	
	public void Save() {
		//todo save provile
	}

	public void Create() {
		//create profile
	}
}