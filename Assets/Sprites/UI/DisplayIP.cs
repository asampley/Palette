using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayIP : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = "My IP: " + GetIP();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Want to have IP shown on pause menu
    public string GetIP()
    {
        return Network.player.ipAddress;
    }
}
