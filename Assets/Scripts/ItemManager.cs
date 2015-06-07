using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

	private bool falling;
	public AudioClip clipDestroing;

	private AudioSource SFX;

	// Use this for initialization
	void Start () {
		falling = false;
		SFX = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool GetAttr() {
		return falling;
	}

	public void SetAttr(bool attr) {
		falling = attr;
	}

	public void sfxDestroingNoPlayer () {
		SFX.clip = clipDestroing;
		SFX.Play();
	}
}
