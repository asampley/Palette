using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedSceneSwitcher : MonoBehaviour {

	public float time;
	public string nextSceneName;

	// Use this for initialization
	void Start () {
		StartCoroutine (JumpToScene ());
	}

	IEnumerator JumpToScene() {
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (nextSceneName);
	}
}
