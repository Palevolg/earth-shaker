using UnityEngine;
using System.Collections;
using SimpleJSON;

public class ResourceManager {

	private static ResourceManager instance;

	private ResourceManager(){
		//constructor. здесь должны загрузится все необходимые данные
	}

	public static ResourceManager GetInstance() {
		if (instance == null)
			instance = new ResourceManager ();
		return instance;
	}

}
