using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour {
	MovieTexture movie;

	public UnityEvent onFinish;

	public bool jumpOnMovieFinish;
	public string nextSceneName;

	// Use this for initialization
	void Start () {
        GUITexture g = GetComponent<GUITexture>();
        movie = (MovieTexture)g.texture;
        movie.Play();

		StartCoroutine(DoAfterMovie ());
    }

	IEnumerator DoAfterMovie() {
		yield return new WaitWhile (() => movie.isPlaying);
		
		onFinish.Invoke ();

		if (jumpOnMovieFinish) {
			if (NetworkPlayerManager.singleton != null && NetworkPlayerManager.singleton.isNetworkActive) {
				Debug.Log ("Switching network scene");
				NetworkPlayerManager.singleton.ServerChangeScene (nextSceneName);
			} else {
				Debug.Log ("Switching local scene");
				SceneManager.LoadScene (nextSceneName);
			}
		}
	}
}
