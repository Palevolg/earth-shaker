using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour {

	[Serializable]
	public struct NamedClips {
		public string name;
		public AudioClip clip;
	}
	
	public NamedClips[] clips;
	public GameObject PrefabSFX;
	Dictionary<string,AudioClip> clipsDictionary = new Dictionary<string,AudioClip>();

	void Awake () {
		//generate dictionary from array
		foreach(NamedClips element in clips) {
			clipsDictionary.Add(element.name,element.clip);
		}

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySFX(string clipName, float volume = 1.0f) {
		if (!clipsDictionary.ContainsKey(clipName)) return;
		AudioClip clipSFX = clipsDictionary[clipName];
		GameObject soundHelper = Instantiate(PrefabSFX);
		AudioSource ASource = soundHelper.GetComponent<AudioSource>();
		ASource.clip = clipSFX;
		ASource.volume = volume;
		ASource.Play();
		Destroy(soundHelper,(clipSFX.length+1.0f));
	}

	public float ClipDuration(string clipName, float addt = 0f) {
		if (!clipsDictionary.ContainsKey(clipName)) return 0f;
		return clipsDictionary[clipName].length+addt;
	}
}
