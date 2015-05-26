using UnityEngine;
using System.Collections;

public class SessionManager {

	private static SessionManager instance;

	public static SessionManager GetInstance() {
		if (instance == null)
			instance = new SessionManager ();
		return instance;
	}
}
