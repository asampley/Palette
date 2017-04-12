using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour {
	public AudioSource red;
	public AudioSource blue;
	public AudioSource green;
	public AudioSource magenta;
	public AudioSource yellow;
	public AudioSource cyan;
	public AudioSource white;

	public Dictionary<PaletteColorID, AudioSource> sources;
	public Dictionary<PaletteColorID, float> targets;

	public float volume = 1;
	public float fadeTime = 3;

	void Awake() {
		sources = new Dictionary<PaletteColorID, AudioSource> ();
		targets = new Dictionary<PaletteColorID, float> ();

		sources [PaletteColorID.RED] = red;
		sources [PaletteColorID.BLUE] = blue;
		sources [PaletteColorID.GREEN] = green;
		sources [PaletteColorID.CYAN] = cyan;
		sources [PaletteColorID.MAGENTA] = magenta;
		sources [PaletteColorID.YELLOW] = yellow;
		sources [PaletteColorID.WHITE] = white;

		foreach (PaletteColorID id in sources.Keys) {
			targets [id] = 0;
		}
	}

	public void Fade() {
		Debug.Log ("Start Fade");
		foreach (PaletteColorID id in sources.Keys) {
			targets [id] = 0;
			StartCoroutine (CoroutineGoToTarget (id));
		}
	}

	public void Solo(PaletteColorID colorID) {
		Fade ();
		if (SceneData.sceneObject.GetComponent<Music> ().musicOn) {
			sources [colorID].volume = volume;
			targets [colorID] = volume;
		}
	}

	IEnumerator CoroutineGoToTarget(PaletteColorID id) {
		Debug.Log ("Started fading music for color " + id);

		while (true) {
			float dVolumeLin = Mathf.Sign (targets [id] - sources [id].volume) * volume * Time.deltaTime / fadeTime;
			float dVolumeAll = targets [id] - sources [id].volume;

			if (Mathf.Abs(dVolumeLin) < Mathf.Abs(dVolumeAll)) {
				sources [id].volume += dVolumeLin;
				yield return new WaitForEndOfFrame ();
			} else {
				sources [id].volume = targets [id];
				Debug.Log ("Finished fading music for color " + id);
				yield break;
			}
		}
	}
}
