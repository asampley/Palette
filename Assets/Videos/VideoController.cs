using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour {
	MovieTexture movie;
	public bool jumpOnMovieFinish;
	public string nextSceneName;

	// Use this for initialization
	void Start () {
        GUITexture g = GetComponent<GUITexture>();
        movie = (MovieTexture)g.texture;
        movie.Play();

		if (jumpOnMovieFinish) {
			StartCoroutine(JumpToNextSceneAfterMovie ());
		}
    }

	IEnumerator JumpToNextSceneAfterMovie() {
		yield return new WaitWhile (() => movie.isPlaying);

		if (Network.isServer) {
			NetworkPlayerManager.singleton.ServerChangeScene (nextSceneName);
		} else {
			SceneManager.LoadScene (nextSceneName);
		}
	}
}
