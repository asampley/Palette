using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public GameObject PauseScreen;

	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown( KeyCode.P ))
		{
			TogglePauseMenu ();	
		}
	}

	public void TogglePauseMenu()
	{
		if (PauseScreen.activeSelf)
		{
			PauseScreen.SetActive(false);
			//Time.timeScale = 1.0f;
		}
		else
		{
			PauseScreen.SetActive(true);
			//Time.timeScale = 0f;
		}

		//Debug.Log("TimeScale: " + Time.timeScale);
	}
}
