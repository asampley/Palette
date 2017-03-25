using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	private Canvas UI;

	// Use this for initialization
	void Start () {
		UI = this.gameObject.GetComponent<Canvas> ();
		//player = GameObject.Find ("Player 0");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown( KeyCode.P ))
		{
			TogglePauseMenu ();	
		}
	}

	public void TogglePauseMenu()
	{
		// not the optimal way but for the sake of readability
		if (UI.enabled)
		{
			UI.enabled = false;
			Time.timeScale = 1.0f;
		}
		else
		{
			UI.enabled = true;
			Time.timeScale = 0f;
		}

		Debug.Log("TimeScale: " + Time.timeScale);
	}
}
