using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public GameObject pauseScreen;
    private Player player;
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown( KeyCode.P ))
		{
			TogglePauseMenu ();	
			Debug.Log ("Pressed Pause");
		}
	}

	public void TogglePauseMenu()
	{
        player = SceneData.sceneObject.GetComponent<LocalPlayer>().localPlayer;
        if (pauseScreen.activeSelf)
		{
			pauseScreen.SetActive(false);
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<LaserController>().enabled = true;
        }
		else
		{
			pauseScreen.SetActive(true);
            player.GetComponent<PlayerController>().enabled = false;
            player.GetComponent<LaserController>().enabled = false;
        }
	}
}
